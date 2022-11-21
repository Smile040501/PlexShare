﻿using PlexShareScreenshare;
using PlexShareScreenshare.Client;
using PlexShareScreenshare.Server;
using System.Drawing;
using System.Reflection;


namespace PlexShareTests.ScreenshareTests
{
    [Collection("Sequential")]
    public class ScreenProcessorTests
    {
        // tests if some images are captured and processed
        [Fact]
        public void TestProcessedFrameNonEmpty()
        {
            ScreenCapturer screenCapturer = new();
            ScreenProcessor screenProcessor = new(screenCapturer);

            // Capturer must be called before Processor
            screenCapturer.StartCapture();
            screenProcessor.StartProcessing();

            Thread.Sleep(1000);

            screenCapturer.StopCapture();
            int v2 = screenProcessor.GetProcessedFrameLength();
            screenProcessor.StopProcessing();

            Assert.True(v2 > 0);
        }

        // checks if processed frame gets cleared or not after we stop processing
        [Fact]
        public void TestCleanup()
        {
            ScreenCapturer screenCapturer = new();
            ScreenProcessor screenProcessor = new(screenCapturer);

            screenCapturer.StartCapture();
            screenProcessor.StartProcessing();

            Thread.Sleep(1000);

            screenCapturer.StopCapture();
            screenProcessor.StopProcessing();

            Assert.True(screenProcessor.GetProcessedFrameLength() == 0);
        }

        // if we process a image with a new bitmap image then it will not change the 
        // original bitmap image
        [Fact]
        public void TestSameImagePixelDiffZero()
        {
            ScreenCapturer screenCapturer = new();
            bool token = false;

            screenCapturer.StartCapture();
            Bitmap? img = screenCapturer.GetImage(ref token);
            screenCapturer.StopCapture();
            Assert.True(img != null);


            Bitmap emptyImage = new Bitmap(img.Width, img.Height);
            Bitmap? tmp = ScreenStitcher.Process(img, emptyImage);
            Assert.True(tmp != null);
            Assert.True(Utils.CompareBitmap(tmp, img));
        }

        // test if the resolution of image changes after we call "SetNewResolution"
        [Fact]
        public void TestResolutionChange()
        {
            ScreenCapturer screenCapturer = new();
            ScreenProcessor screenProcessor = new(screenCapturer);

            screenCapturer.StartCapture();
            screenProcessor.StartProcessing();

            Thread.Sleep(1000);

            Resolution? res1 = (Resolution?)typeof(ScreenProcessor)
                .GetField("_currentRes", BindingFlags.NonPublic | BindingFlags.Instance)!
                .GetValue(screenProcessor);

            Assert.NotNull(res1);

            screenProcessor.SetNewResolution(9);

            Thread.Sleep(1000);

            Resolution? res2 = (Resolution?)typeof(ScreenProcessor)
                .GetField("_currentRes", BindingFlags.NonPublic | BindingFlags.Instance)!
                .GetValue(screenProcessor);

            Assert.NotNull(res2);

            screenCapturer.StopCapture();
            screenProcessor.StopProcessing();

            Assert.True(res1?.Height / 9 == res2?.Height);
            Assert.True(res1?.Width / 9 == res2?.Width);
        }

        // test if image processed has 0 or 1 at the end
        [Fact]
        public void TestCorrectImageStringFormat()
        {
            ScreenCapturer screenCapturer = new();
            ScreenProcessor screenProcessor = new(screenCapturer);

            screenCapturer.StartCapture();
            screenProcessor.StartProcessing();

            int cnt = 5;
            while (cnt-- > 0)
            {
                bool token = false;
                string tmpStr = screenProcessor.GetFrame(ref token);
                Assert.True(tmpStr[^1] == '0' || tmpStr[^1] == '1');
            }

            screenCapturer.StopCapture();
            screenProcessor.StopProcessing();
        }
    }
}
