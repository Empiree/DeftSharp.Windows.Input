using System.Linq.Expressions;
using System.Linq;
using Xunit;

namespace DeftSharp.Windows.Input;

public sealed class PolyFillTest
{

    [Fact]
    public async void TakeLast_1()
    {
        int TakeLast_count = 1;
        var que = new Queue<int>(new[] { 1, 2, 3, 4, 5 });

        var val = Enumerable.TakeLast(que, TakeLast_count).ToList();  // net8 original Linq
        var valpf = PolyFillExtensions.TakeLast(que, TakeLast_count).ToList();

        Assert.True(val[0] == valpf[0]);
        //await Task.Run(() => { });
    }

    [Fact]
    public async void TakeLast_2()
    {
        int TakeLast_count = 2;
        var que = new Queue<int>(new[] { 1, 2, 3, 4, 5 });

        var val = Enumerable.TakeLast(que, TakeLast_count).ToList();  // net8 original Linq
        var valpf = PolyFillExtensions.TakeLast(que, TakeLast_count).ToList();

        Assert.True(val[0] == valpf[0]);
        Assert.True(val[1] == valpf[1]);
    }

    [Fact]
    public async void TakeLast_3()
    {
        int TakeLast_count = 10;
        var que = new Queue<int>(Enumerable.Range(1,100));
        
        var val = Enumerable.TakeLast(que, TakeLast_count).ToList();  // net8 original Linq
        var valpf = PolyFillExtensions.TakeLast(que, TakeLast_count).ToList();

        Assert.True(val[0] == valpf[0]);
        Assert.True(val[4] == valpf[4]);
        Assert.True(val[9] == valpf[9]);
    }


}