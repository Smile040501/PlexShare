using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenShare.Client
{
    // Singleton Class
    internal class ScreenShareController
    {
        ScreenShareController() {}

        public void StartSharing()
        {
            // Calls `StartCapture` and `StartProcess`
        }

        public void SuspendSharing()
        {
            // Calls `SuspendCapture` and `SuspendProcessing`
        }

        public void ResumeSharing()
        {
            // Calls `ResumeCapture` and `ResumeProcessing`
        }

        public void StopSharing()
        -{
            // Calls `StopCapture` and `StopProcessing`
        }

        public void SetOldResolution()
        {
            // pass
        }

        public void SetNewResolution()
        {
            // update the new resolution to be used by the Processor
        }
    }
}
