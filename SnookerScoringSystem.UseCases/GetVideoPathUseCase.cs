using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnookerScoringSystem.UseCases.Interfaces;
using SnookerScoringSystem.UseCases.PluginInterfaces;

namespace SnookerScoringSystem.UseCases
{
    public class GetVideoPathUseCase : IGetVideoPathUseCase
    {
        private readonly IVideoProcessingRepository _videoProcessingRepository;

        public GetVideoPathUseCase(IVideoProcessingRepository videoProcessingRepository)
        {
            this._videoProcessingRepository = videoProcessingRepository;
        }
        public string Execute()
        {
            return _videoProcessingRepository.GetVideoPath();
        }
    }
}