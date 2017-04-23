Athame
======
Athame is a program for downloading music from music streaming and sharing services.
It is intended for educational and private use only, and **not** as a tool for pirating and distributing music.
Above all else, remember that the artists and studios put a lot of work into making music -- if you can, purchase
music directly from the artist as more money goes to them.

Since I am also caught up with other things I can't devote all my time to fixing and improving Athame. Right now it is
just a very buggy, basic tool which I hope will either be improved upon in the future, or be contributed to.

Notes about testing services and development
--------------------------------------------
Unfortunately I can't pay for both Tidal and Google Play Music, so I am unable to test Google Play Music functionality.

Latest release
--------------
The code in the current repo probably won't compile, so check out the releases tab to download a pre-built binary.

Usage
-----
Enter a URL in the "URL" textbox, then click "Add". It will show up in the download queue. Click "Start" to begin downloading.

Currently, Athame only supports Tidal and Google Play Music URLs. This will eventually be upgraded to a generic plugin
interface, and the Google Play Music and Tidal services will be in different repositories.

If you haven't signed in, you can click the `Menu` button, then go to `Settings` and choose the tab of the music service
you want to sign into. You can also just enter a URL and click "Click here to sign in" on the error message below the URL
textbox.

Under `Settings > General`, you can change where music is downloaded to as well as the filename format used. There is an explanation
of the valid format specifiers on the General tab.

TODO
----
* Implement a plugin API.
* Add an option to execute an external program (like `ffmpeg`) after each track or collection download.
* Properly implement cancellation and stopping on sign in and download.
* Implement search.

Build/requirements
------------------
* .NET 4.6.2 (for other platforms see Mono compatibility section)
* Visual Studio 2015 (Express will work fine) with NuGet

Mono compatibility
------------------
For the most part, it appears to work on Mono on Linux (tested on Linux Mint 18). However, there are a few bugs you need to be aware of, namely:

* Signing in via clicking the "Click here to sign in" link will crash for some reason.
* You should avoid adding more than one set of media to the doiwnload queue as it will only download the first set then hang.
* The UI will not be disabled when downloading, so try and avoid clicking on shit while it downloads.

Linux/OS X compatibility isn't my top priority at the moment, but you are welcome to contribute if you like. I have been thinking about adding a CLI interface,
but I think a WPF and GTK# UI would be more pertinent at the moment.