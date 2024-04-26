using SnookerScoringSystem.Domain;
using Compunet.YoloV8;
using Compunet.YoloV8.Data;
using SnookerScoringSystem.UseCases.PluginInterfaces;
using System.Diagnostics;

namespace SnookerScoringSystem.Plugins.Datastore.Model
{
    // All the code in this file is included in all platforms.
    public class ModelRepository : ISnookerDetectionModelRepository 
    {
        private readonly YoloV8Predictor _predictor;
        private readonly string _modelPath;


        //Create yoloV8 predictor by passing the path to the model file. 
        public ModelRepository()
        {
            try
            {
                _modelPath = GetModelPath();
                _predictor = YoloV8Predictor.Create(_modelPath );


            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while creating the object detection model: {ex.Message}", ex);
            }
        }

        //Find the path to model file
        private string GetModelPath()
        {
            var modelPath = Path.Combine(AppContext.BaseDirectory, "Models", "SnookerDetectionModel.onnx");

            if (!File.Exists(modelPath))
            {
                throw new FileNotFoundException($"The model file cannot be found at path: {modelPath}");
            }

            return modelPath;
        }


        //Ball detection function, pass an image to predictor
        public async Task<List<DetectedBall>> DetectSnookerBallAsync(string framePath)
        {
            if (string.IsNullOrWhiteSpace(framePath))
            {
                throw new ArgumentException("Image path cannot be null or empty.", nameof(framePath));
            }

            // Check if the image file exists
            if (!File.Exists(framePath))
            {
                throw new FileNotFoundException("The image file was not found.", framePath);
            }

            try
            {
                var results = await _predictor.DetectAsync(framePath);
                var detectedBalls = new List<DetectedBall>();

                foreach (var result in results.Boxes.ToList())
                {
                    detectedBalls.Add(new DetectedBall
                    {
                        ClassId = result.Class.Id,
                        ClassName = result.Class.Name.Replace(",", ""),
                        X = result.Bounds.X,
                        Y = result.Bounds.Y,
                        Width = result.Bounds.Width,
                        Height = result.Bounds.Height
                    });
                }
                

                return detectedBalls;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while detecting snooker balls");
            }
        }
    }
}
