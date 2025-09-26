using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UltraDES;
using System.IO;


namespace PFC_Final
{
    class Program
    {
        static void Main(string[] args)
        {
            #region PequenaFabrica

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
                new Transition(S21, a2, S22),
                new Transition(S22, b2, S21)
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

            #endregion

            #region SIDIM
            /*
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


            supervisor.Add(Supervisor);




            INOGenerator.ConvertDEStoINO(planta, supervisor);


            */

            #endregion



            List<DeterministicFiniteAutomaton> supervisor = new List<DeterministicFiniteAutomaton>();
            List<DeterministicFiniteAutomaton> planta = new List<DeterministicFiniteAutomaton>();

            planta.Add(G1);
            planta.Add(G2);

            supervisor.Add(Supervisor);

            INOGenerator.ConvertDEStoINO(planta, supervisor);

        }

        private static void FSM(out List<DeterministicFiniteAutomaton> plants, out List<DeterministicFiniteAutomaton> specs)
        {
            var s = new List<State>(); // or State[] s = new State[6];
            for (var i = 0; i < 6; i++)
                s.Add(i == 0 ? new State(i.ToString(), Marking.Marked) : new State(i.ToString()));

            // Creating Events (0 to 100)
            var e = new List<Event>(); // or Event[] e = new Event[100];
            for (var i = 0; i < 100; ++i)
                e.Add(i % 2 != 0
                    ? new Event($"e{i}", Controllability.Controllable)
                    : new Event($"e{i}", Controllability.Uncontrollable));

            //----------------------------
            // Plants
            //----------------------------


            // C1
            var transC1 = new List<Transition>();
            transC1.Add(new Transition(s[0], e[11], s[1]));
            transC1.Add(new Transition(s[1], e[12], s[0]));

            var c1 = new DeterministicFiniteAutomaton(transC1, s[0], "C1");

            // C2
            var c2 = new DeterministicFiniteAutomaton(
                new[]
                {
                    new Transition(s[0], e[21], s[1]),
                    new Transition(s[1], e[22], s[0])
                },
                s[0], "C2");

            // Milling
            var milling = new DeterministicFiniteAutomaton(
                new[]
                {
                    new Transition(s[0], e[41], s[1]),
                    new Transition(s[1], e[42], s[0])
                },
                s[0], "Milling");

            // MP
            var mp = new DeterministicFiniteAutomaton(
                new[]
                {
                    new Transition(s[0], e[81], s[1]),
                    new Transition(s[1], e[82], s[0])
                },
                s[0], "MP");

            // Lathe
            var lathe = new DeterministicFiniteAutomaton(
                new[]
                {
                    new Transition(s[0], e[51], s[1]),
                    new Transition(s[1], e[52], s[0]),
                    new Transition(s[0], e[53], s[2]),
                    new Transition(s[2], e[54], s[0])
                },
                s[0], "Lathe");

            // C3
            var c3 = new DeterministicFiniteAutomaton(
                new[]
                {
                    new Transition(s[0], e[71], s[1]),
                    new Transition(s[1], e[72], s[0]),
                    new Transition(s[0], e[73], s[2]),
                    new Transition(s[2], e[74], s[0])
                },
                s[0], "C3");

            // Robot
            var robot = new DeterministicFiniteAutomaton(
                new[]
                {
                    new Transition(s[0], e[31], s[1]),
                    new Transition(s[1], e[32], s[0]),
                    new Transition(s[0], e[33], s[2]),
                    new Transition(s[2], e[34], s[0]),
                    new Transition(s[0], e[35], s[3]),
                    new Transition(s[3], e[36], s[0]),
                    new Transition(s[0], e[37], s[4]),
                    new Transition(s[4], e[38], s[0]),
                    new Transition(s[0], e[39], s[5]),
                    new Transition(s[5], e[30], s[0])
                },
                s[0], "Robot");

            // MM
            var mm = new DeterministicFiniteAutomaton(
                new[]
                {
                    new Transition(s[0], e[61], s[1]),
                    new Transition(s[1], e[63], s[2]),
                    new Transition(s[1], e[65], s[3]),
                    new Transition(s[2], e[64], s[0]),
                    new Transition(s[3], e[66], s[0])
                },
                s[0], "MM");

            //----------------------------
            // Specifications
            //----------------------------

            // E1
            var e1 = new DeterministicFiniteAutomaton(
                new[]
                {
                    new Transition(s[0], e[12], s[1]),
                    new Transition(s[1], e[31], s[0])
                },
                s[0], "E1");

            // E2
            var e2 = new DeterministicFiniteAutomaton(
                new[]
                {
                    new Transition(s[0], e[22], s[1]),
                    new Transition(s[1], e[33], s[0])
                },
                s[0], "E2");

            // E5
            var e5 = new DeterministicFiniteAutomaton(
                new[]
                {
                    new Transition(s[0], e[36], s[1]),
                    new Transition(s[1], e[61], s[0])
                },
                s[0], "E5");

            // E6
            var e6 = new DeterministicFiniteAutomaton(
                new[]
                {
                    new Transition(s[0], e[38], s[1]),
                    new Transition(s[1], e[63], s[0])
                },
                s[0], "E6");

            // E3
            var e3 = new DeterministicFiniteAutomaton(
                new[]
                {
                    new Transition(s[0], e[32], s[1]),
                    new Transition(s[1], e[41], s[0]),
                    new Transition(s[0], e[42], s[2]),
                    new Transition(s[2], e[35], s[0])
                },
                s[0], "E3");

            // E7
            var e7 = new DeterministicFiniteAutomaton(
                new[]
                {
                    new Transition(s[0], e[30], s[1]),
                    new Transition(s[1], e[71], s[0]),
                    new Transition(s[0], e[74], s[2]),
                    new Transition(s[2], e[65], s[0])
                },
                s[0], "E7");

            // E8
            var e8 = new DeterministicFiniteAutomaton(
                new[]
                {
                    new Transition(s[0], e[72], s[1]),
                    new Transition(s[1], e[81], s[0]),
                    new Transition(s[0], e[82], s[2]),
                    new Transition(s[2], e[73], s[0])
                },
                s[0], "E8");

            // E4
            var e4 = new DeterministicFiniteAutomaton(
                new[]
                {
                    new Transition(s[0], e[34], s[1]),
                    new Transition(s[1], e[51], s[0]),
                    new Transition(s[1], e[53], s[0]),
                    new Transition(s[0], e[52], s[2]),
                    new Transition(s[2], e[37], s[0]),
                    new Transition(s[0], e[54], s[3]),
                    new Transition(s[3], e[39], s[0])
                },
                s[0], "E4");

            plants = new[] { c1, c2, milling, lathe, robot, mm, c3, mp }.ToList();
            specs = new[] { e1, e2, e3, e4, e5, e6, e7, e8 }.ToList();
        }

    }
}
