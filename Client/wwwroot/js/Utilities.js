function timerNoActive(dotnetHelper, seconds)
{
    var timer;
    var timeInSeconds = seconds;

    document.onmousemove=resetTimer;
    document.onkeypress=resetTimer;
    timer = setTimeout(logout, timeInSeconds);

    function resetTimer()
    {
        clearTimeout(timer);
        timer = setTimeout(logout, timeInSeconds);
    }
    function logout()
    {
        dotnetHelper.invokeMethodAsync("Logout");
    }
}


    window.blazorCulture = {
        get: () => window.localStorage['BlazorCulture'],
            set: (value) => window.localStorage['BlazorCulture'] = value
};





