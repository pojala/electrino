# electrino

A desktop runtime for apps built on web technologies, using the system's own web browser engine.

Electrino is an experimental featherweight alternative to the popular and powerful [Electron](https://github.com/electron/electron). It implements a minuscule portion of the APIs available in Electron, but the output app size is much smaller.

A "Hello World" app takes 115 MB using Electron, but only 167 kB using Electrino:

![Screenshot from Mac Finder](docs/electron-and-electrino-helloworld-screenshot.png)

This comparison is completely unfair because Electrino currently doesn't do anything more. You can load a web page, access some basic Electron/Node APIs, and that's it. Electrino is nothing more than a proof of concept at present.

The current implementation is only for macOS. A Windows port using Microsoft Edge seems potentially interesting though.

Read more about Electrino in this post on DailyJS:
https://medium.com/dailyjs/put-your-electron-app-on-a-diet-with-electrino-c7ffdf1d6297

### Roadmap

The plan is to examine API usage of real-world apps that use Electron but don't really need the full capabilities. Good candidates are desktop utilities, menu bar apps and other small apps that users typically leave open. (For large productivity-style apps, Electron is a better choice.)

Jan Hovancik offered his [Stretchly](https://github.com/hovancik/stretchly) app as a candidate, so I'm going to start by mapping out the APIs used by Stretchly and see what it would take to implement it with Electrino.

If you have a small Electron-based Mac app and you'd like to try putting it on an Electrino diet, let's give it a try! My contact info is below.

### Contact

Pauli Olavi Ojala / @pauliooj / pauli @ lacquer.fi
