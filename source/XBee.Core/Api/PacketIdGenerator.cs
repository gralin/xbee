using System;

namespace Gadgeteer.Modules.GHIElectronics.Api
{
    /// <summary>
    /// Generates packet id numbers to be used with XBeeRequest.
    /// </summary>
    public class PacketIdGenerator
    {
        private int _sequentialFrameId = 0xff;

        public int GetCurrentFrameId()
        {
            return _sequentialFrameId;
        }

        /// <summary>
        /// This is useful for obtaining a frame id when composing your XBeeRequest. 
        /// It will return frame ids in a sequential manner until the maximum is reached (0xff)
        /// and it flips to 1 and starts over.
        /// </summary>
        /// <returns></returns>
        public int GetNextFrameId()
        {
            if (_sequentialFrameId == 0xff)
            {
                // flip
                _sequentialFrameId = 1;
            }
            else
            {
                _sequentialFrameId++;
            }

            return _sequentialFrameId;
        }

        /// <summary>
        /// Updates the frame id.
        /// </summary>
        /// <param name="val">Any value between 1 and ff is valid</param>
        public void UpdateFrameId(int val)
        {
            if (val <= 0 || val > 0xff)
                throw new ArgumentException("invalid frame id");

            _sequentialFrameId = val;
        }
    }
}