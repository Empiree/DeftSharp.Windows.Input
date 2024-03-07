using System.Windows.Input;
using DeftSharp.Windows.Input.Keyboard;

namespace DeftSharp.Windows.Input.Tests.Keyboard;

public class ManipulatorTests : IDisposable
{
    private readonly WPFEmulator _emulator;

    public ManipulatorTests()
    {
        _emulator = new WPFEmulator();
    }

    [Fact]
    public void Manipulator_Test1()
    {
        var keyboardManipulator1 = new KeyboardManipulator();
        var keyboardManipulator2 = new KeyboardManipulator();

        _emulator.Run(() =>
        {
            keyboardManipulator1.Prevent(Key.A);
            keyboardManipulator2.Prevent(Key.A);
            
            Assert.Equal(keyboardManipulator1.LockedKeys.Count(), keyboardManipulator2.LockedKeys.Count());
            
            keyboardManipulator1.Release(Key.A);
        });
    }
    
    [Fact]
    public void Manipulator_Test2()
    {
        var keyboardManipulator1 = new KeyboardManipulator();
        var keyboardManipulator2 = new KeyboardManipulator();

        _emulator.Run(() =>
        {
            keyboardManipulator1.Prevent(Key.A);
            keyboardManipulator2.Prevent(Key.A);
            
            keyboardManipulator1.Release(Key.A);
        });
    }
    
    [Fact]
    public void Manipulator_Test3()
    {
        var keyboardManipulator1 = new KeyboardManipulator();
        var keyboardManipulator2 = new KeyboardManipulator();
        var keyboardManipulator3 = new KeyboardManipulator();

        _emulator.Run(() =>
        {
            keyboardManipulator1.Prevent(Key.Q);
            keyboardManipulator2.Prevent(Key.W);
            keyboardManipulator3.Prevent(Key.R);

            keyboardManipulator1.Release(Key.Q);
            keyboardManipulator2.Release(Key.W);
            keyboardManipulator3.Release(Key.R);
        });
    }
    
    [Fact]
    public void Manipulator_Test4()
    {
        var keyboardManipulator1 = new KeyboardManipulator();
        var keyboardManipulator2 = new KeyboardManipulator();
        var keyboardManipulator3 = new KeyboardManipulator();

        _emulator.Run(() =>
        {
            keyboardManipulator1.Prevent(Key.Q);
            keyboardManipulator2.Prevent(Key.W);

            keyboardManipulator3.Release(Key.Q);
            keyboardManipulator3.Release(Key.W);
        });
    }
    
    [Fact]
    public void Manipulator_Test5()
    {
        _emulator.Run(() =>
        {
            using (var keyboardManipulator1 = new KeyboardManipulator())
            {
                using (var keyboardManipulator2 = new KeyboardManipulator())
                {
                    using (var keyboardManipulator3 = new KeyboardManipulator())
                    {
                        keyboardManipulator1.Prevent(Key.Q);
                        keyboardManipulator2.Prevent(Key.W);
                    }
                    
                    keyboardManipulator1.Release(Key.Q);
                    keyboardManipulator1.Release(Key.W);
                }
            }
        });
    }
    
    [Fact]
    public void Manipulator_Test6()
    {
        var counterOfPreventEvents = 0;
        var counterOfReleasedEvents = 0;
        var keyboardManipulator1 = new KeyboardManipulator();
        _emulator.Run(() =>
        {
            var keyboardManipulator2 = new KeyboardManipulator();
            keyboardManipulator2.KeyPrevented += key =>
            {
                counterOfPreventEvents++;
            };
            
            keyboardManipulator1.Prevent(Key.A);
            keyboardManipulator1.Prevent(Key.A);
            
            var keyboardManipulator3 = new KeyboardManipulator();
            keyboardManipulator3.Prevent(Key.E);

            keyboardManipulator3.KeyReleased += key =>
            {
                counterOfReleasedEvents++;
            };
            
            Assert.Equal(2, counterOfPreventEvents);
            
            keyboardManipulator3.Release(Key.A);
            keyboardManipulator3.Release(Key.E);
            
            Assert.Equal(counterOfReleasedEvents, counterOfPreventEvents);
        });
    }

    public void Dispose()
    {
        var manipulator = new KeyboardManipulator();
        Assert.Empty(manipulator.LockedKeys);
    }
}
