using System;
using System.Collections.Generic;

namespace DOTE.Gameplay.Domain.Field
{
    public struct Hex
    {
        public int Q { get; private set; }
        public int R { get; private set; }
        public int S { get; private set; }

        private static List<Hex> diagonals = new List<Hex>
        {
            new Hex(2, -1, -1), new Hex(1, -2, 1), new Hex(-1, -1, 2),
            new Hex(-2, 1, 1), new Hex(-1, 2, -1), new Hex(1, 1, -2)
        };

        private static List<Hex> directions => new List<Hex>
        {
            new Hex(1, 0, -1), new Hex(1, -1, 0), new Hex(0, -1, 1),
            new Hex(-1, 0, 1), new Hex(-1, 1, 0), new Hex(0, 1, -1)
        };

        public Hex(int q, int r) : this()
        {
            Q = q;
            R = r;
            S = 1;
            if (Q + R + S != 0) throw new ArgumentException("q + r + s must be 0");
        }

        public Hex(int q, int r, int s)
        {
            if (q + r + s != 0) throw new ArgumentException("q + r + s must be 0");
            this.Q = q;
            this.R = r;
            this.S = s;
        }

        public Hex Add(Hex b)
        {
            return new Hex(Q + b.Q, R + b.R, S + b.S);
        }


        public Hex Subtract(Hex b)
        {
            return new Hex(Q - b.Q, R - b.R, S - b.S);
        }


        public Hex Scale(int k)
        {
            return new Hex(Q * k, R * k, S * k);
        }


        public Hex RotateLeft()
        {
            return new Hex(-S, -Q, -R);
        }


        public Hex RotateRight()
        {
            return new Hex(-R, -S, -Q);
        }


        public override bool Equals(object obj)
        {
            return obj is Hex hex &&
                   Q == hex.Q &&
                   R == hex.R &&
                   S == hex.S;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Q, R, S);
        }


        public Hex Neighbor(int direction)
        {
            return Add(Direction(direction));
        }

        public List<Hex> Neighbors()
        {
            List<Hex> neighbors = new();
            for (int i = 0; i < directions.Count; i++)
            {
                neighbors.Add(Direction(i));
            }
            return neighbors;
        }

        public Hex DiagonalNeighbor(int direction)
        {
            return Add(Hex.diagonals[direction]);
        }


        public int Length()
        {
            return (Math.Abs(Q) + Math.Abs(R) + Math.Abs(S)) / 2;
        }

        public int Distance(Hex b)
        {
            return Subtract(b).Length();
        }

        public static Hex operator +(Hex a, Hex b)
        {
            return a.Add(b);
        }

        public static Hex operator -(Hex a, Hex b)
        {
            return a.Subtract(b);
        }

        public static Hex operator *(Hex a, int k)
        {
            return a.Scale(k);
        }

        public static bool operator ==(Hex a, Hex b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Hex a, Hex b)
        {
            return !(a == b);
        }

        public static Hex Direction(int direction)
        {
            return Hex.directions[direction];
        }

        public static Hex Direction(int direction, int range)
        {
            return Direction(direction).Scale(range);
        }

    }

    struct FractionalHex
    {
        public FractionalHex(double q, double r, double s)
        {
            this.q = q;
            this.r = r;
            this.s = s;
            if (Math.Round(q + r + s) != 0) throw new ArgumentException("q + r + s must be 0");
        }
        public readonly double q;
        public readonly double r;
        public readonly double s;

        public Hex HexRound()
        {
            int qi = (int)(Math.Round(q));
            int ri = (int)(Math.Round(r));
            int si = (int)(Math.Round(s));
            double q_diff = Math.Abs(qi - q);
            double r_diff = Math.Abs(ri - r);
            double s_diff = Math.Abs(si - s);
            if (q_diff > r_diff && q_diff > s_diff)
            {
                qi = -ri - si;
            }
            else
                if (r_diff > s_diff)
            {
                ri = -qi - si;
            }
            else
            {
                si = -qi - ri;
            }
            return new Hex(qi, ri, si);
        }


        public FractionalHex HexLerp(FractionalHex b, double t)
        {
            return new FractionalHex(q * (1.0 - t) + b.q * t, r * (1.0 - t) + b.r * t, s * (1.0 - t) + b.s * t);
        }


        public static List<Hex> HexLinedraw(Hex a, Hex b)
        {
            int N = a.Distance(b);
            FractionalHex a_nudge = new FractionalHex(a.Q + 1e-06, a.R + 1e-06, a.S - 2e-06);
            FractionalHex b_nudge = new FractionalHex(b.Q + 1e-06, b.R + 1e-06, b.S - 2e-06);
            List<Hex> results = new List<Hex> { };
            double step = 1.0 / Math.Max(N, 1);
            for (int i = 0; i <= N; i++)
            {
                results.Add(a_nudge.HexLerp(b_nudge, step * i).HexRound());
            }
            return results;
        }

    }
}