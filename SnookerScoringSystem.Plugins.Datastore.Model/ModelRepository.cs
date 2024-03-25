using SnookerScoringSystem.Domain;
using Compunet.YoloV8;
using Compunet.YoloV8.Data;
using SnookerScoringSystem.UseCases.PluginInterfaces;

namespace SnookerScoringSystem.Plugins.Datastore.Model
{
    // All the code in this file is included in all platforms.
    public class ModelRepository : ISnookerDetectionModelRepository 
    {
        private readonly YoloV8Predictor _predictor;
        private readonly string _modelPath;


        public ModelRepository()
        {
            try
            {
                _modelPath = Path.Combine(AppContext.BaseDirectory, "Models", "SnookerDetectionModel.onnx");

                if (!File.Exists(_modelPath))
                {
                    throw new Exception("The model file cannot be found");
                }

                _predictor = YoloV8Predictor.Create(_modelPath );
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while creating the object detection model");
            }
        }

        private string extractBallColour(BoundingBox detectedBoundingBox)
        {
            string ballColor = "";
            string ball = detectedBoundingBox.ToString().Trim('\"');
            int commaIndex = ball.IndexOf(',');
            if (commaIndex != -1)
            {
                ballColor = ball.Substring(0, commaIndex);
            }

            return ballColor;
        }

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
                        ClassName = extractBallColour(result),
                        X1 = result.Bounds.X
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
