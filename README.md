# _The Arena_ (previous title: _The Wall_)

_The Arena_ is an Action RPG developed in Unity by [Ryan Donnelly](https://bitbucket.org/ryanfd/), [Alexandre Guichet](https://bitbucket.org/Salade_de_Poney/), [Keegan Landrigan](https://bitbucket.org/keeganland/), [Patric McConnell](https://bitbucket.org/PMc42/), and Elo�se Zahabi. It began as a project in association with the [UBC AMS Game Development Association](http://www.amsgda.com/).

Special thanks to [Gowtham Mohan](https://bitbucket.org/Gowtham100/) for helping to get this project started.

## About _The Arena_

_The Arena_ plays like a Real-Time Strategy/RPG hybrid, along the lines of _Warcraft 3_ or contemporary MOBAs like _League of Legends_. Part of the inspiration comes from games like the _The Legend of Zelda: Four Swords_ sub-series, which involve multiple characters co-operating to fight boss monsters.

Much of the project has involved expanding our familiarity with team-based software development and software engineering outside of the scope available to us in a classroom setting. Furthermore, it involved acquiring significant number of technical skills not directly encountered in the classroom, such as proficiency with C#, Unity, and the Visual Studio IDE. Work on it has largely been an educational, exploratory experience as we discover for ourselves the capabilities of this toolset.

The final length of the game amounts to a short demo: 3 boss fights in the Arena itself and an additional fight in a dungeon area.

## Credits

Artwork in _The Arena_ is largely stock artwork taken from _RPG Maker MV_. 

## Technical Q&A

### Q: Running _The Arena_ within Unity works fine; why I am getting an error when I try to build the project?
A: /Assets/Scripts/MakeWeaponObject.cs is a script necessary for development processes, but causes problems at build time. It may be harmlessly surpressed when you trying to build. 