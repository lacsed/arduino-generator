using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UltraDES;

namespace PFC_Final
{
    class INOGenerator
    {

        public static void ConvertDEStoINO(List<DeterministicFiniteAutomaton> plantList, List<DeterministicFiniteAutomaton> supervisorList)
        {
            List<DeterministicFiniteAutomaton> automatonList = new List<DeterministicFiniteAutomaton>();
            automatonList.AddRange(plantList);
            automatonList.AddRange(supervisorList);

            int numberOfAutomatons = automatonList.Count;

            #region Read default files

            string defaultPath = @"..\..\INOdefaut\";
            string dotINO = File.ReadAllText(defaultPath + "mainDefault.ino");
            string dotH = File.ReadAllText(defaultPath + "AutomatonDefault.h");
            string dotCPP = File.ReadAllText(defaultPath + "AutomatonDefault.cpp");
            string dotUser = File.ReadAllText(defaultPath + "UserFunctionsDefault.cpp");
            string dotEventCPP = File.ReadAllText(defaultPath + "EventDefault.cpp");
            string dotEventH = File.ReadAllText(defaultPath + "EventDefault.h");

            #endregion

            #region Initialize StringBuilder variables

            StringBuilder stateAction = new StringBuilder();
            StringBuilder trasionLogic = new StringBuilder();
            StringBuilder automatonLoop = new StringBuilder();
            StringBuilder automatonLoopForAll = new StringBuilder();
            StringBuilder stateActionForAll = new StringBuilder();
            StringBuilder makeTrasition = new StringBuilder();
            StringBuilder vectorAction = new StringBuilder();
            StringBuilder instanceAutomaton = new StringBuilder();
            StringBuilder stateEventVector = new StringBuilder();
            StringBuilder assignmentVector = new StringBuilder();
            StringBuilder uncontrollableEvent = new StringBuilder();
            StringBuilder uncontrollableEventForAll = new StringBuilder();
            StringBuilder getEventUncontrollable = new StringBuilder();
            StringBuilder defineEventPosition = new StringBuilder();

            #endregion

            #region Event Translate

            List<AbstractEvent> listOfEvents = new List<AbstractEvent>();

            foreach (var automaton in automatonList)
            {
                var newEvents = automaton.Events.Except(listOfEvents);
                listOfEvents.AddRange(newEvents);
            }

            Dictionary<AbstractEvent, string> eventMap = new Dictionary<AbstractEvent, string>();
            Dictionary<AbstractEvent, string> eventMapPosition = new Dictionary<AbstractEvent, string>();

            int numEvents = listOfEvents.Count;
            int sizeVector = (numEvents + (numEvents % 8)) / 8;


            foreach (var element in listOfEvents)
            {
                int i = listOfEvents.IndexOf(element);
                defineEventPosition.AppendLine($"#define EVENT_{element.ToString().ToUpper()} {numEvents-1-i}");
                StringBuilder sequence = new StringBuilder(numEvents);
                sequence.Append('0', numEvents);
                sequence[i] = '1';
                eventMap[element] = SplitBinaryString(sequence.ToString());
                eventMapPosition[listOfEvents[i]] = $"EVENT_{element.ToString().ToUpper()}";
            }

            getEventUncontrollable.AppendLine($"void getEventUncontrollable(Event &eventUncontrollable){{");


            foreach (var ev in listOfEvents)
            {

                if (!ev.IsControllable)
                {
                    uncontrollableEvent.AppendLine($"bool EventUncontrollable{ev.ToString()}();");

                    uncontrollableEventForAll.AppendLine($"bool EventUncontrollable{ev.ToString()}(){{");
                    uncontrollableEventForAll.AppendLine($"\treturn false;");
                    uncontrollableEventForAll.AppendLine($"}}\n");

                    getEventUncontrollable.AppendLine($"\tif(EventUncontrollable{ev.ToString()}()){{");
                    getEventUncontrollable.AppendLine($"\t\tsetBit(eventUncontrollable,{eventMapPosition[ev]}, true);");
                    getEventUncontrollable.AppendLine($"\t}}");


                }


            }
            getEventUncontrollable.AppendLine($"}}");

            #endregion

            #region Plant Translate

            int autnum = 0;

            ConvertAutomatonList(plantList, eventMap, eventMapPosition, instanceAutomaton, automatonLoopForAll, automatonLoop, trasionLogic, stateActionForAll, stateAction, stateEventVector, makeTrasition, assignmentVector, sizeVector, ref autnum, "automata");


            #endregion

            #region Supervisor Translate

            ConvertAutomatonList(supervisorList, eventMap, eventMapPosition, instanceAutomaton, automatonLoopForAll, automatonLoop, trasionLogic, stateActionForAll, stateAction, stateEventVector, makeTrasition, assignmentVector, sizeVector, ref autnum, "supervisor");


            #endregion

            #region Replace Text 

            dotH = dotH.Replace("// ADD-STATE-ACTION", stateAction.ToString());
            dotH = dotH.Replace("// ADD-TRANSITION-LOGIC", trasionLogic.ToString());
            dotH = dotH.Replace("// ADD-AUTOMATON-LOOP", automatonLoop.ToString());
            dotH = dotH.Replace("// ADD-EVENT-UNCONTROLLABLE", uncontrollableEvent.ToString());

            dotCPP = dotCPP.Replace("// ADD-SET-VECTOR", assignmentVector.ToString());
            dotCPP = dotCPP.Replace("// ADD-VECTOR-ACTION", vectorAction.ToString());
            dotCPP = dotCPP.Replace("// ADD-AUTOMATON-LOOP", automatonLoopForAll.ToString());
            dotCPP = dotCPP.Replace("// ADD-TRANSITION-LOGIC", makeTrasition.ToString());
            dotCPP = dotCPP.Replace("#include \"AutomatonDefault.h\"", "#include \"Automaton.h\"");
            dotCPP = dotCPP.Replace("#include \"EventDefault.h\"", "#include \"Event.h\"");

            dotINO = dotINO.Replace("#include \"AutomatonDefaut.h\"", "#include \"Automaton.h\"");
            dotINO = dotINO.Replace("#include \"EventDefault.h\"", "#include \"Event.h\"");
            dotINO = dotINO.Replace("#define NUMBER_AUTOMATON 1", $"#define NUMBER_AUTOMATON {plantList.Count}");
            dotINO = dotINO.Replace("#define NUMBER_SUPERVISOR 1", $"#define NUMBER_SUPERVISOR {supervisorList.Count}");
            dotINO = dotINO.Replace("// ADD-INSTANCE-AUTOMATON", instanceAutomaton.ToString());
            dotINO = dotINO.Replace("// ADD-VECTOR-EVENT", stateEventVector.ToString());

            dotUser = dotUser.Replace("// ADD-STATE-ACTION", stateActionForAll.ToString());
            dotUser = dotUser.Replace("// ADD-GET-EVENT-UNCONTROLLABLE", getEventUncontrollable.ToString());
            dotUser = dotUser.Replace("// ADD-EVENT-UNCONTROLLABLE", uncontrollableEventForAll.ToString());
            dotUser = dotUser.Replace("#include \"AutomatonDefault.h\"", "#include \"Automaton.h\"");

            dotEventH = dotEventH.Replace("// ADD-ALL-EVENTS", defineEventPosition.ToString());
            dotEventH = dotEventH.Replace("#include \"EventDefault.h\"", "#include \"Event.h\"");
            dotEventH = dotEventH.Replace("#define SIZE_EVENT 1", $"#define SIZE_EVENT {sizeVector}");
            dotEventH = dotEventH.Replace("#define NUMBER_EVENT 1", $"#define NUMBER_EVENT {numEvents}");

            dotEventCPP = dotEventCPP.Replace("#include \"EventDefault.h\"", "#include \"Event.h\"");

            #endregion

            #region Write All Result Files

            string folderName = $"main-{DateTime.Now.ToString("dd-MMM-yyyy-HH-mm-ss")}";
            string resultPath = @"..\..\INOresult\" + folderName;

            Directory.CreateDirectory(resultPath);

            File.WriteAllText(resultPath + @"\" + "Event.cpp", dotEventCPP);
            File.WriteAllText(resultPath + @"\" + "Event.h", dotEventH);
            File.WriteAllText(resultPath + @"\" + "UserFunctions.cpp", dotUser);
            File.WriteAllText(resultPath + @"\" + "Automaton.cpp", dotCPP);
            File.WriteAllText(resultPath + @"\" + "Automaton.h", dotH);
            File.WriteAllText(resultPath + @"\" + $"{folderName}.ino", dotINO);

            #endregion

        }
        private static void ConvertAutomatonList(List<DeterministicFiniteAutomaton> automatonList, Dictionary<AbstractEvent, string> eventMap, Dictionary<AbstractEvent, string> eventMapPosition, StringBuilder instanceAutomaton, StringBuilder automatonLoopForAll, StringBuilder automatonLoop, StringBuilder trasionLogic, StringBuilder stateActionForAll, StringBuilder stateAction, StringBuilder stateEventVector, StringBuilder makeTrasition, StringBuilder assignmentVector, int sizeVector, ref int autnumLocal, string typeAutomaton)
        {
            int numberOfAutomatons = automatonList.Count;

            int autnum = autnumLocal;
            int countAut = 0;

            bool isSupervisor = typeAutomaton == "supervisor";

            foreach (var automaton in automatonList)
            {
                int numberStates = automaton.States.Count();

                automatonLoopForAll.AppendLine($"void Automaton{autnum}Loop(int State){{");
                automatonLoopForAll.AppendLine($"\tActionAutomatons{autnum}[State]();");
                automatonLoopForAll.AppendLine("}\n");

                automatonLoop.AppendLine($"void Automaton{autnum}Loop(int State); \n");
                trasionLogic.Append($"int MakeTransitionAutomaton{autnum}(int State, Event eventOccurred);\n");

                if (!isSupervisor)
                {
                    for (int i = 0; i < numberStates; i++)
                    {
                        string stateName = $"StateActionAutomaton{autnum}State{i}";

                        stateActionForAll.Append($"void {stateName}()\n{{\n\tSerial.println(\"A{autnum}S{i}\");\n \tdelay(500);\n}}\n\n");
                        stateAction.Append($"void {stateName}();\n");
                    }

                    stateAction.Append("\n");
                }

                // State Translate
                Dictionary<AbstractState, int> stateMap = new Dictionary<AbstractState, int>();

                int numStates = automaton.States.Count();

                int count = 0;

                List<string> listEventOfState = new List<string>();

                var allEvent = automaton.Events.Select(t => eventMap[t]).ToList();
                string alphabet = ConcatenateBinaryStrings(allEvent);


                foreach (var state in automaton.States)
                {
                    var stateTransitions = automaton.Transitions.Where(t => t.Origin.Equals(state)).ToList();
                    var events = stateTransitions.Select(t => eventMap[t.Trigger]).ToList();

                    events.Add(InvertBinaryString(alphabet));

                    string eventsString = ConcatenateBinaryStrings(events);
                    
                    listEventOfState.Add(eventsString);

                    stateMap[state] = count;
                    count++;
                }

                StringBuilder eventHandler = new StringBuilder();

                eventHandler.AppendLine(string.Join("\n", listEventOfState.Select((item, index) => $"uint8_t eventDataAUT{autnum}EV{index}[{sizeVector}] = {item};")));

                stateEventVector.AppendLine(eventHandler.ToString());
                stateEventVector.Append($"Event enabledEventStatesAutomaton{autnum}[{count}] = {{");
                stateEventVector.Append(string.Join(",", listEventOfState.Select((item, index) => $" createEventFromData(eventDataAUT{autnum}EV{index})")));
                stateEventVector.Append($"}};\n");

                makeTrasition.AppendLine($"int MakeTransitionAutomaton{autnum}(int State, Event eventOccurred) \n{{ ");

                foreach (var (o, ev, d) in automaton.Transitions)
                {
                    makeTrasition.AppendLine($"\tif (State == {stateMap[o]} && (getBit(eventOccurred,{eventMapPosition[ev]}))){{ ");
                    makeTrasition.AppendLine($"\t\treturn {stateMap[d]};");
                    makeTrasition.AppendLine("\t}");


                }

                makeTrasition.AppendLine("\n\treturn(State);");
                makeTrasition.AppendLine("}\n");

                instanceAutomaton.Append("\n" + $"\t\t{typeAutomaton}[{countAut}] = ").Append(
                    $"Automaton({numberStates}," +
                    $"enabledEventStatesAutomaton{autnum}," +
                    $"&MakeTransitionAutomaton{autnum}," +
                    $"&Automaton{autnum}Loop);");


                if (isSupervisor)
                {
                    assignmentVector.AppendLine($"GenericAction ActionAutomatons{autnum}[{1}]={{[](){{}}}};");
                }
                else
                {
                    assignmentVector.AppendLine($"GenericAction ActionAutomatons{autnum}[{numberStates}]={{ {string.Join(",", Enumerable.Range(0, numberStates).Select(i => $"&StateActionAutomaton{autnum}State{i}"))} }};");
                }

                countAut++;
                autnum++;
            }
            autnumLocal = autnum;
        }

        
        private static string ConcatenateBinaryStrings(List<string> input)
        {
            int sizeString = input[0].Length;
            StringBuilder binaryString = new StringBuilder();

            for (int i = 0; i < sizeString; i++)
            {
                char resultChar = '0'; 

                foreach (var item in input)
                {
                    char currentChar = item[i];

                    if (currentChar != '0' && currentChar != '1')
                    {
                        resultChar = currentChar; 
                        break; 
                    }
                    else 
                    {
                        if (currentChar == '1')
                        {
                            resultChar = '1';
                            break;
                        }

                        resultChar = '0';

                    }
                }

                binaryString.Append(resultChar);
            }

            return binaryString.ToString();
        }
        private static string SplitBinaryString(string input)
        {
            int size = 8;
            input = Regex.Replace(input, "[^01]", "");

            int padding = input.Length % size;
            if (padding > 0)
            {
                input = input.PadLeft(input.Length + (size - padding), '0');
            }

            StringBuilder result = new StringBuilder();
            int position = 0;

            while (position < input.Length)
            {
                string chunk = input.Substring(position, size);

                result.Append($"0b{chunk}");
                position += size;

                if (position < input.Length)
                {
                    result.Append(", ");
                }


            }

            return "{" + result.ToString() + "}";
        }
        private static string InvertBinaryString(string input)
        {
            char[] chars = input.ToCharArray();
            bool inverter = false; 

            for (int i = 0; i < input.Length; i++)
            {
                if (chars[i] == 'b' || chars[i] == ',')
                {
                    inverter = !inverter; 
                }
                else if (inverter && (chars[i] == '0' || chars[i] == '1'))
                {
                    chars[i] = (chars[i] == '0') ? '1' : '0'; 
                }
            }

            return new string(chars);
        }
    }
}
