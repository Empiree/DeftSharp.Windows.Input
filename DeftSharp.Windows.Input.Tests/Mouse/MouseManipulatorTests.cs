namespace DeftSharp.Windows.Input.Tests.Mouse
{
    public class MouseManipulatorTests : IDisposable
    {
        [Fact]
        public async void MouseManipulator_PreventBy2()
        {
            var mouseManipulator1 = new MouseManipulator();
            var mouseManipulator2 = new MouseManipulator();

            await Task.Run(() =>
            {
                mouseManipulator1.Prevent(PreventMouseOption.MiddleButton);
                mouseManipulator2.Prevent(PreventMouseOption.MiddleButton);

                Assert.Equal(mouseManipulator1.LockedKeys.Count(), mouseManipulator2.LockedKeys.Count());

                mouseManipulator1.Release(PreventMouseOption.MiddleButton);
            });
        }

        [Fact]
        public async void MouseManipulator_PreventReleaseAll()
        {
            var mouseManipulator1 = new MouseManipulator();
            var mouseManipulator2 = new MouseManipulator();

            await Task.Run(() =>
            {
                mouseManipulator1.Prevent(PreventMouseOption.MiddleButton);
                mouseManipulator2.Prevent(PreventMouseOption.MiddleButton);

                Assert.Equal(mouseManipulator1.LockedKeys.Count(), mouseManipulator2.LockedKeys.Count());

                mouseManipulator1.Release();

                Assert.Empty(mouseManipulator2.LockedKeys);
            });
        }

        [Fact]
        public async void MouseManipulator_PreventBy3()
        {
            var mouseManipulator1 = new MouseManipulator();
            var mouseManipulator2 = new MouseManipulator();
            var mouseManipulator3 = new MouseManipulator();

            await Task.Run(() =>
            {
                mouseManipulator1.Prevent(PreventMouseOption.MiddleButton);
                mouseManipulator2.Prevent(PreventMouseOption.RightButton);
                mouseManipulator3.Prevent(PreventMouseOption.Scroll);

                Assert.Equal(mouseManipulator1.LockedKeys.Count(), mouseManipulator2.LockedKeys.Count());
                Assert.Equal(mouseManipulator2.LockedKeys.Count(), mouseManipulator3.LockedKeys.Count());

                mouseManipulator1.Release();

                Assert.Empty(mouseManipulator2.LockedKeys);
            });
        }

        [Fact]
        public async void MouseManipulator_PreventBy2ReleaseAll()
        {
            var mouseManipulator1 = new MouseManipulator();
            var mouseManipulator2 = new MouseManipulator();
            var mouseManipulator3 = new MouseManipulator();

            await Task.Run(() =>
            {
                mouseManipulator1.Prevent(PreventMouseOption.MiddleButton);
                mouseManipulator2.Prevent(PreventMouseOption.RightButton);

                mouseManipulator3.Release();

                Assert.Empty(mouseManipulator1.LockedKeys);
                Assert.Empty(mouseManipulator2.LockedKeys);
            });
        }

        [Fact]
        public async void MouseManipulator_DisposeTest()
        {
            await Task.Run(() =>
            {
                using (var mouseManipulator1 = new MouseManipulator())
                {
                    using (var mouseManipulator2 = new MouseManipulator())
                    {
                        using (var mouseManipulator3 = new MouseManipulator())
                        {
                            mouseManipulator1.Prevent(PreventMouseOption.MiddleButton);
                            mouseManipulator2.Prevent(PreventMouseOption.RightButton);
                        }

                        Assert.NotEmpty(mouseManipulator1.LockedKeys);

                        mouseManipulator1.Release(PreventMouseOption.MiddleButton);
                        mouseManipulator1.Release(PreventMouseOption.RightButton);
                    }
                }
            });
        }

        public void Dispose()
        {
            var manipulator = new MouseManipulator();
            Assert.Empty(manipulator.LockedKeys);
        }
    }
}
