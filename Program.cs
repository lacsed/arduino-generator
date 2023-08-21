using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltraDES;


namespace PFC_Final
{
    class Program
    {
        static void Main(string[] args)
        {

            // Estados da Maquina 1
            State S11 = new State("S11", Marking.Marked);
            State S12 = new State("S12", Marking.Unmarked);

            // Estados da Maquina 2
            State S21 = new State("S21", Marking.Marked);
            State S22 = new State("S22", Marking.Unmarked);

            // Estados do Buffer com uma posicao
            State SB1 = new State("Empty", Marking.Marked);
            State SB2 = new State("Full", Marking.Unmarked);

            // Eventos controlaveis
            Event a1 = new Event("a1", Controllability.Controllable);
            Event a2 = new Event("a2", Controllability.Controllable);

            // Eventos nao controlaveis
            Event b1 = new Event("b1", Controllability.Uncontrollable);
            Event b2 = new Event("b2", Controllability.Uncontrollable);

            // Automato Maquina 1
            var G1 = new DeterministicFiniteAutomaton(new[]{
                new Transition(S11, a1, S12),
                new Transition(S12, b1, S11)
              }, S11, "G1");

            // Automato Maquina 2 
            var G2 = new DeterministicFiniteAutomaton(new[]{
                new Transition(S21, a2, SB2),
                new Transition(SB2, b2, S21)
              }, S21, "G2");

            // Automato Buffer (Especificacao)
            var E = new DeterministicFiniteAutomaton(new[]{
                new Transition(SB1, b1, SB2),
                new Transition(SB2, a2, SB1)
              }, SB1, "E");

            var Planta = G1.ParallelCompositionWith(G2);

            var Supervisor = DeterministicFiniteAutomaton.MonolithicSupervisor(
                    new[] { G1, G2 },
                    new[] { E },
                    true
                        );

            List<DeterministicFiniteAutomaton> planta = new List<DeterministicFiniteAutomaton>();
            List<DeterministicFiniteAutomaton> supervisor = new List<DeterministicFiniteAutomaton>();


            planta.Add(G1);
            planta.Add(G2);
            planta.Add(E);
            planta.Add(Planta);

            supervisor.Add(Supervisor);

            INOGenerator.ConvertDEStoINO(planta, supervisor);






        }
    }
}
