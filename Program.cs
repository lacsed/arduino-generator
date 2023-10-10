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

            //// Estados da Maquina 1
            //State S11 = new State("S11", Marking.Marked);
            //State S12 = new State("S12", Marking.Unmarked);

            //// Estados da Maquina 2
            //State S21 = new State("S21", Marking.Marked);
            //State S22 = new State("S22", Marking.Unmarked);

            //// Estados do Buffer com uma posicao
            //State SB1 = new State("Empty", Marking.Marked);
            //State SB2 = new State("Full", Marking.Unmarked);

            //// Eventos controlaveis
            //Event a1 = new Event("a1", Controllability.Controllable);
            //Event a2 = new Event("a2", Controllability.Controllable);

            //// Eventos nao controlaveis
            //Event b1 = new Event("b1", Controllability.Uncontrollable);
            //Event b2 = new Event("b2", Controllability.Uncontrollable);

            //// Automato Maquina 1
            //var G1 = new DeterministicFiniteAutomaton(new[]{
            //    new Transition(S11, a1, S12),
            //    new Transition(S12, b1, S11)
            //  }, S11, "G1");

            //// Automato Maquina 2 
            //var G2 = new DeterministicFiniteAutomaton(new[]{
            //    new Transition(S21, a2, SB2),
            //    new Transition(SB2, b2, S21)
            //  }, S21, "G2");

            //// Automato Buffer (Especificacao)
            //var E = new DeterministicFiniteAutomaton(new[]{
            //    new Transition(SB1, b1, SB2),
            //    new Transition(SB2, a2, SB1)
            //  }, SB1, "E");

            //var Planta = G1.ParallelCompositionWith(G2);

            //var Supervisor = DeterministicFiniteAutomaton.MonolithicSupervisor(
            //        new[] { G1, G2 },
            //        new[] { E },
            //        true
            //            );

               //Estados
            var s0 = new State("s0", Marking.Marked);
            var s1 = new State("s1", Marking.Unmarked);
            var s2 = new State("s2", Marking.Unmarked);
            var s3 = new State("s3", Marking.Unmarked);
            var s4 = new State("s4", Marking.Unmarked);

            //Eventos
            var e =
               Enumerable.Range(0, 17)
                   .Select(i =>
                       new Event($"E_{i}",
                           i % 2 != 0
                               ? Controllability.Controllable
                               : Controllability.Uncontrollable)
                   ).ToArray();

            //Plantas
            var R = new DeterministicFiniteAutomaton(
              new[]
              {
                    new Transition(s0, e[1], s1),
                    new Transition(s1, e[2], s0)
              },
              s0, "R");

            var C1 = new DeterministicFiniteAutomaton(
              new[]
              {
                    new Transition(s0, e[3], s1),
                    new Transition(s1, e[14], s0)
              },
              s0, "C1");

            var C2 = new DeterministicFiniteAutomaton(
              new[]
              {
                    new Transition(s0, e[9], s1),
                    new Transition(s1, e[10], s0)
              },
              s0, "C2");

            var C3 = new DeterministicFiniteAutomaton(
              new[]
              {
                    new Transition(s0, e[11], s1),
                    new Transition(s1, e[12], s0)
              },
              s0, "C3");

            var Cg = new DeterministicFiniteAutomaton(
              new[]
              {
                    new Transition(s0, e[5], s1),
                    new Transition(s1, e[6], s0),
                    new Transition(s0, e[7], s2),
                    new Transition(s2, e[8], s0)
              },
              s0, "Cg");

            //Especificações
            var E1 = new DeterministicFiniteAutomaton(
              new[]
              {
                    new Transition(s0, e[2], s1),
                    new Transition(s1, e[3], s0)
              },
              s0, "E1");

            var E2 = new DeterministicFiniteAutomaton(
              new[]
              {
                    new Transition(s0, e[12], s3),
                    new Transition(s3, e[7], s0),
                    new Transition(s0, e[14], s4),
                    new Transition(s4, e[5], s0)
              },
              s0, "E2");

            var E3 = new DeterministicFiniteAutomaton(
              new[]
              {
                    new Transition(s0, e[6], s1),
                    new Transition(s1, e[9], s0)
              },
              s0, "E3");

            var E4 = new DeterministicFiniteAutomaton(
              new[]
              {
                    new Transition(s0, e[8], s1),
                    new Transition(s1, e[11], s0)
              },
              s0, "E4");


            //Supervisor monolítico
            var SM = DeterministicFiniteAutomaton.MonolithicSupervisor(new[] { R, C1, C2, C3, Cg }, new[] { E1, E2, E3, E4 });




            List<DeterministicFiniteAutomaton> planta = new List<DeterministicFiniteAutomaton>();
            List<DeterministicFiniteAutomaton> supervisor = new List<DeterministicFiniteAutomaton>();


            planta.Add(R);
            planta.Add(C1);
            planta.Add(C2);
            planta.Add(C3);
            planta.Add(Cg);

            supervisor.Add(SM);

         


            INOGenerator.ConvertDEStoINO(planta, supervisor);

   





        }
    }
}
