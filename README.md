# _The Arena_ (previous title: _The Wall_)

_The Arena_ is an Action RPG developed in Unity in association with Alexandre Guichet, Patric McConnell, Ryan Donnelly, and Eloïse Zahabi. It began as a project in association with the UBC AMS Game Development Association.

## Technical Q&A

### Q: Running _The Arena_ within Unity works fine; why I am getting an error when I try to build the project?
A: /Assets/Scripts/MakeWeaponObject.cs is a script necessary for development processes, but causes problems at build time. It may be harmlessly surpressed when you trying to build. 