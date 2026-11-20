namespace Csag.OrchardCoreModules.PageViewStats.Services;

public interface IBotDetector
{
    bool CheckUserAgentString(string userAgentString);
}