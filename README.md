# UltraDES Extension: Finite State Automaton to Arduino Converter
## Overview

This repository includes an extension to the [UltraDes library](https://github.com/lacsed/UltraDES), which introduces a new feature for transforming finite state automata from discrete event systems into Arduino files. This feature is the INOGenerator program, aimed at streamlining the conversion of Deterministic Finite Automata (DFA) from UltraDES into Arduino-compatible **.ino** files. These generated files cooperate to faithfully represent the system's behavior on the Arduino platform with the purpose of this program is to simplify the translation of DFA-based models into executable code suitable for Arduino environments.

## Background
The [UltraDes library](https://github.com/lacsed/UltraDES) is a powerful tool for modeling discrete event systems and their behaviors. However, it lacks a straightforward way to implement these behaviors on hardware platforms such as Arduino. This extension aims to bridge this gap by offering a function that transforms finite state automata created using the UltraDes library into Arduino-compatible code


## Installation
1. Make sure you have the [UltraDes library](https://github.com/lacsed/UltraDES) installed. If not, you can find it here.
2. Clone this repository to your local machine or download the ZIP file.
3. Open the solution in your preferred C# development environment.
4. Locate the INOGenerator class within the **PFC_Final** namespace.
5. Use the **ConvertDEStoINO** method to perform the DFA-to-INO conversion. This method takes two lists as arguments: one for plant automata and another for supervisor automata.
6. After executing the conversion, the resulting Arduino-compatible **.ino**, **.h**, and **.cpp** files will be generated and stored in the specified output directory.

## Contributions
Contributions to the INOGenerator program are welcome. If you identify any issues, want to suggest improvements, or wish to contribute new features, please feel free to open an issue or submit a pull request in the [arduino-generator repository](https://github.com/lacsed/arduino-generator).

## License
The INOGenerator program is distributed under the MIT License.



