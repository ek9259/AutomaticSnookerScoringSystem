using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnookerScoringSystem.UseCases.Interfaces;
using SnookerScoringSystem.UseCases.PluginInterfaces;

namespace SnookerScoringSystem.UseCases
{
    public class ExtractFrameUseCase : IExtractFrameUseCase
    {
        private readonly IVideoProcessingRepository _videoProcessingRepository;

        public ExtractFrameUseCase(IVideoProcessingRepository videoProcessingRepository)
        {
            this._videoProcessingRepository = videoProcessingRepository;
        }

        public async Task ExecuteAsync()
        {
            await _videoProcessingRepository.ExtractFrameAsync();
        }

        
    }
}
