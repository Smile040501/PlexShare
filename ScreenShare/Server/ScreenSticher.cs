using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenShare.Server
{
    internal class ScreenStitcher
    {
        private SharedClientScreen _sharedClientScreen;

        private Task _stitchTask;

        // Called by the `SharedClientScreen`
        ScreenStitcher(SharedClientScreen scs) { }

        // Creates(if not exist) and start the task `_stitchTask`
        // Will read the image using `_sharedClientScreen.GetImage`
        // and puts the final image using `_sharedClientScreen.PutFinalImage`
        public void StartStitching() { }

        // Kills the task `_stitchTask`
        public void StopStitching() { }

        // Function to stitch the new image over the old image
        // Frame: new_res, list of : { x, y, (R, G, B)}
        private Bitmap Stitch(prevFrame, frame);
    }
}
