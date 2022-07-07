mergeInto(LibraryManager.library, 
{
    OpenWindow: function(link)
    {
        var url = UTF8ToString(link);
        console.log("URL: " + url);
        window.open(url);
    }
});