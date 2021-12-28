# XAML Framework Performance Comparison

Thanks to Microsoft's recent work on their Maui cross-platform graphics library, creating a mostly apples to apples performance test has been made much easier.

Description: 

> !! The original run of this benchmark was flawed !!  Both UWP and WinUI take a performance hit when attached to Visual Studio 2019.

This project compares the performance of three XAML frameworks: UWP, WPF (.NET 6.0) and WinUI (v 1.0 as of December 28, 2021)

Test cases were adjusted somewhat because Maui is not yet equivalent in all drawing operations across the different XAML frameworks.  The work done on the Windows Community Toolkit demonstrates that it is possible to do equivalent drawing operations in UWP and WinUI, but those approaches are not yet reflected in the code of Maui.

This is not an exaustive performance test, but it does touch on the core capabilities of the XAML frameworks being tested.

## Results

## (  UWP  ) Elapsed: 10004 ms, Passes: 1200
## (  WPF  ) Elapsed: 10164 ms, Passes: 1200
## ( WinUI ) Elapsed: 13530 ms, Passes: 1200

A higher elapsed time results in a slower UI rendering interval.  WinUI is roughly 30% times slower than UWP and WPF.  Release build memory utilization for the benchmark maps out as follows: WPF ~85MB, UWP ~30MB, WinUI ~58MB.  Because WinUI is still unfinished, we can expect major improvements in its memory footprint.

## License same as original

https://github.com/dotnet/Microsoft.Maui.Graphics/blob/main/LICENSE

## Credits and Ideas

https://github.com/dotnet/Microsoft.Maui.Graphics

## Caveats

A detailed analysis of whether this test is an exact 1:1 performance comparison has not been done.  It is assumed that Microsoft has done a good job of architecting the framework specific rendering code.

If you examine the code, you will see an almost equivalent test implementation for WPF, UWP and WinUI.  UWP and WinUI are in fact 1:1 in every respect.

## Notes

Preliminary results show that UWP has the fastest XAML rendering layer.  WPF has a very respectable showing. WPF in .NET6 with R2R scores nearly as much as UWP.

WinUI is slower in comparison to WPF and UWP. But it had major improvments in last months and right now it's doing a pretty decent job.  Presumably, WinUI will have a lot of under the hood tuning to match UWP and WPF performance.

Microsoft has been boasting about how comparable WinUI is to UWP.  WinUI needs to narrow the performance and feature gap when compared to UWP to gain wider adoption from existing UWP developers.

WPF developers looking for a fresh look and the prospect of future feature gains may want to start looking at WinUI despite its performance deficit.
