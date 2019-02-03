# _The Arena_ (previous title: _The Wall_)

_The Arena_ is an Action RPG developed in Unity by [Ryan Donnelly](https://bitbucket.org/ryanfd/), [Alexandre Guichet](https://bitbucket.org/Salade_de_Poney/), [Keegan Landrigan](https://bitbucket.org/keeganland/), [Patric McConnell](https://bitbucket.org/PMc42/), and Eloïse Zahabi. It began as a project in association with the [UBC AMS Game Development Association](http://www.amsgda.com/).

Special thanks to [Gowtham Mohan](https://bitbucket.org/Gowtham100/) for helping to get this project started.

![](ImagesGitReadme/TheArena.PNG)

## Technical Q&A

### Q: Running _The Arena_ within Unity works fine; why I am getting an error when I try to build the project?
A: /Assets/Scripts/MakeWeaponObject.cs is a script necessary for development processes, but causes problems at build time. It may be harmlessly surpressed when you trying to build. 
