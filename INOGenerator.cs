using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

            for (int i = 0; i < numEvents; i++)
            {
                defineEventPosition.AppendLine($"#define EVENT_{listOfEvents[i].ToString().ToUpper()} {i}");
                StringBuilder sequence = new StringBuilder(numEvents);
                sequence.Append('0', numEvents);
                sequence[i] = '1';
                eventMap[listOfEvents[i]] = sequence.ToString();
                eventMapPosition[listOfEvents[i]] = $"EVENT_{listOfEvents[i].ToString().ToUpper()}";
            }

            getEventUncontrollable.AppendLine($"unsigned long getEventUncontrollable(){{");
            getEventUncontrollable.AppendLine($"\tunsigned long Uncontrollable=0; \n");

            
            foreach (var ev in listOfEvents)
            {

                if (!ev.IsControllable)
                {
                    uncontrollableEvent.AppendLine($"bool EventUncontrollable{ev.ToString()}();");

                    uncontrollableEventForAll.AppendLine($"bool EventUncontrollable{ev.ToString()}(){{");
                    uncontrollableEventForAll.AppendLine($"\treturn false;");
                    uncontrollableEventForAll.AppendLine($"}}\n");

                    getEventUncontrollable.AppendLine($"\tif(EventUncontrollable{ev.ToString()}()){{");
                    getEventUncontrollable.AppendLine($"\t\tUncontrollable|=0b{eventMap[ev]};");
                    getEventUncontrollable.AppendLine($"\t}}");

                    
                }
              

            }
            getEventUncontrollable.AppendLine($"\n\treturn Uncontrollable;");
            getEventUncontrollable.AppendLine($"}}");

            #endregion

            #region Plant Translate

            int autnum = 0;

            ConvertAutomatonList(plantList, eventMap, eventMapPosition, instanceAutomaton, automatonLoopForAll, automatonLoop, trasionLogic,  stateActionForAll,  stateAction, stateEventVector, makeTrasition, assignmentVector, ref autnum, "automata");


            #endregion

            #region Supervisor Translate

            ConvertAutomatonList(supervisorList, eventMap, eventMapPosition, instanceAutomaton, automatonLoopForAll, automatonLoop, trasionLogic, stateActionForAll, stateAction, stateEventVector, makeTrasition, assignmentVector, ref autnum, "supervisor");


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
            dotINO = dotINO.Replace("#define NUMBER_AUTOMATON 1", $"#define NUMBER_AUTOMATON {plantList.Count}");
            dotINO = dotINO.Replace("#define NUMBER_SUPERVISOR 1", $"#define NUMBER_SUPERVISOR {supervisorList.Count}");
            dotINO = dotINO.Replace("#define NUMBER_EVENT 1", $"#define NUMBER_EVENT {numEvents}");
            dotINO = dotINO.Replace("// ADD-INSTANCE-AUTOMATON", instanceAutomaton.ToString());
            dotINO = dotINO.Replace("// ADD-VECTOR-EVENT", stateEventVector.ToString());



            dotUser = dotUser.Replace("// ADD-STATE-ACTION", stateActionForAll.ToString());
            dotUser = dotUser.Replace("// ADD-GET-EVENT-UNCONTROLLABLE", getEventUncontrollable.ToString());
            dotUser = dotUser.Replace("// ADD-EVENT-UNCONTROLLABLE", uncontrollableEventForAll.ToString());
            dotUser = dotUser.Replace("#include \"AutomatonDefault.h\"", "#include \"Automaton.h\"");

            dotEventH = dotEventH.Replace("// ADD-ALL-EVENTS", defineEventPosition.ToString());

            
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

        private static void ConvertAutomatonList(List<DeterministicFiniteAutomaton> automatonList, Dictionary<AbstractEvent, string> eventMap, Dictionary<AbstractEvent, string> eventMapPosition, StringBuilder instanceAutomaton, StringBuilder automatonLoopForAll, StringBuilder automatonLoop, StringBuilder trasionLogic, StringBuilder stateActionForAll, StringBuilder stateAction, StringBuilder stateEventVector, StringBuilder makeTrasition, StringBuilder assignmentVector, ref int autnumLocal, string typeAutomaton)
        {
            int numberOfAutomatons = automatonList.Count;

            int autnum = autnumLocal;

            instanceAutomaton.AppendLine($"Automaton {typeAutomaton}[{numberOfAutomatons}] = {{");

            bool isSupervisor = typeAutomaton == "supervisor"; 

            foreach (var automaton in automatonList)
            {
                int numberStates = automaton.States.Count();

                automatonLoopForAll.AppendLine($"void Automaton{autnum}Loop(int State){{");
                automatonLoopForAll.AppendLine($"\tActionAutomatons{autnum}[State]();");
                automatonLoopForAll.AppendLine("}\n");

                automatonLoop.AppendLine($"void Automaton{autnum}Loop(int State); \n");
                trasionLogic.Append($"int MakeTransitionAutomaton{autnum}(int State, uint8_t eventPosition);\n");

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



                foreach (var state in automaton.States)
                {
                    var stateTransitions = automaton.Transitions.Where(t => t.Origin.Equals(state)).ToList();
                    var events = stateTransitions.Select(t => eventMap[t.Trigger]).ToList();

                    string eventsString = ConcatListString(events);
                    listEventOfState.Add(eventsString);

                    stateMap[state] = count;
                    count++;
                }



                stateEventVector.Append($"unsigned long enabledEventStatesAutomaton{autnum}[{count}]={{");
                stateEventVector.Append(string.Join(",", listEventOfState.Select(item => $" 0b{item}")));
                stateEventVector.Append($"}};\n");


                makeTrasition.AppendLine($"int MakeTransitionAutomaton{autnum}(int State, uint8_t eventPosition) \n{{ ");
            
                foreach (var (o, ev, d) in automaton.Transitions)
                {
                    makeTrasition.AppendLine($"\tif (State == {stateMap[o]} && (eventPosition=={eventMapPosition[ev]})){{ ");
                    makeTrasition.AppendLine($"\t\treturn {stateMap[d]};");
                    makeTrasition.AppendLine("\t}");


                }

                makeTrasition.AppendLine("\n\treturn(State);");
                makeTrasition.AppendLine("}\n");



                instanceAutomaton.Append($"\t\t\t\t\tAutomaton({numberStates}," +
                $"enabledEventStatesAutomaton{autnum},&MakeTransitionAutomaton{autnum},&Automaton{autnum}Loop)");

                if (autnum + 1 < numberOfAutomatons)
                {
                    instanceAutomaton.Append(", \n");
                }







                if (isSupervisor)
                {
                    assignmentVector.AppendLine($"GenericAction ActionAutomatons{autnum}[{1}]={{[](){{}}}};");
                }
                else
                {
                    assignmentVector.AppendLine($"GenericAction ActionAutomatons{autnum}[{numberStates}]={{ {string.Join(",", Enumerable.Range(0, numberStates).Select(i => $"&StateActionAutomaton{autnum}State{i}"))} }};");
                }



                autnum++;
            }

            autnumLocal = autnum;
            instanceAutomaton.AppendLine("\n};");
        }

        private static string ConcatListString(List<string> eventsString)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < eventsString[0].Length; i++)
            {
                int result = 0;
                foreach (string str in eventsString)
                {
                    result |= (str[i] - '0');
                }
                sb.Append(result);
            }

            return sb.ToString();
        }




    }
}
