## Ability
* Allows you to Copy (Ctrl+C) Cut (Ctrl+X) Paste (Crtl+V) in Unity project (file) view. (aka project explorer window)
* Allows you to do Windows explorer-like navigation, Back (Alt+Left) Forward (Alt+Right) Up one level (Alt+Up) in Unity project (file) view.
* **I'm too lazy to record cool gif to demonstrate this, I hope you get the idea.**
## Install (Package manager GIT method)
* Make sure you have git client installed on your PC, preferably "Git for Windows" https://git-scm.com/download/win
  * If you are using this method for team project git, everyone on your team should have git client or they will have warning message on opening a project.
  * Specialized git client such as bit bucket / source tree **does not** counted as git client by Unity editor, etc. And still get warning.
  * Install using direct method instead if you are not sure.
* From Unity, go to Window-> Package manager
* Plus button on upper left (?), choose from git URL
* Enter https://github.com/wappenull/unity-cutcopypaste-nav.git
* Done, go ahead, and try it.
## Install (Direct method)
* This will install as local package. 
  * Without use and need of git client on PC. 
  * Difference with git method is this wont be able to update/auto-update via package manager window.
  * Commit this local package along with your project file to your version control to let everyone have it.
* Download this repo.
* Extract content to `"YourUnityProject/Packages/com.talecrafter-wappen.copycutpaste"`
* Effectively, **package.json** file will be positioned at `"YourUnityProject/Packages/com.talecrafter-wappen.copycutpaste/package.json"`
* Done, go ahead, and try it.
## Known issue
* Nav feature: is explictly for two-column mode only. CHECKMATE! one-column-er!
* Nav feature: does not offer UI back button, only keyboard shortcut for now.
* Nav feature: Might still buggy, esp when deleting stuff. (please report issue?)
* Not tested against every unity version in the world.
## Credit and acknowledgement
* Original CopyCutPaste was created by Stephan Hövelbrinks | https://twitter.com/talecrafter/status/834000968261984256
* Modified by Wappen to include explorer navigation | http://github.com/wappenull/
* ~~Steal~~ Er, I mean used some code and inspriation from https://github.com/acoppes/unity-history-window Thank you!
## License
You can freely use/distribute this plugin in any way possible with the exception of selling it on it's own.
