# XAML Framework Performance Comparison

Thanks to Microsoft's recent work on their Maui cross-platform graphics library, creating a mostly apples to apples performance test has been made much easier.

Description: 

This project compares the performance of three XAML frameworks: WPF, UWP and WinUI

Test cases were adjusted somewhat because Maui is not yet equivalent in all drawing operations across the different XAML frameworks.  The work done on the Windows Community Toolkit demonstrates that it is possible to do equivalent drawing operations in UWP and WinUI, but those approaches are not yet reflected in the code of Maui.

This is not an exaustive performance test, but it does touch on the core capabilities of the XAML frameworks being tested.

## Results

#(  WPF  ) Elapsed: 16997 ms, Passes: 1200
#(  UWP  ) Elapsed: 18865 ms, Passes: 1200
#( WinUI ) Elapsed: 67067 ms, Passes: 1200

A higher elapsed time results in a slower UI rendering interval.  WinUI is roughly 3 times slower than WPF and UWP.

## License same as original

https://github.com/dotnet/Microsoft.Maui.Graphics/blob/main/LICENSE

## Credits and Ideas

https://github.com/dotnet/Microsoft.Maui.Graphics

## Caveats

A detailed analysis of whether this test is an exact 1:1 performance comparison has not been done.  It is assumed that Microsoft has done a good job of architecting the framework specific rendering code.

If you examine the code, you will see an almost equivalent test implementation for WPF, UWP and WinUI.  UWP and WinUI are in fact 1:1 in every respect.

## Notes

Preliminary results show that WPF outperforms UWP and WinUI in release and debug builds.  The gap between WPF and UWP is much narrower for a release build.

Yes, I was surprised by that.

Unfortunately, WinUI is not yet in the running.  It is extremely sluggish in comparison to WPF and UWP.  Presumably, WinUI still needs a lot of under the hood tuning.

Microsoft has been boasting about how comparable WinUI is to UWP.  Clearly, WinUI has a significant performance and feature gap when compared to UWP.

The good news, the performance of WPF using .Net 3.x and 5.x has seen drastic improvements.
