﻿using Hippie.Unchecked;
using System;
using System.Runtime.CompilerServices;

namespace C_Sharp_Challenge_Skeleton.Answers
{
    public class Question6
    {
        /* when in doubt, roll your own priority queue */
        class State : IComparable<State>
        {
            public int p, n;
            public State(int _p, int _n)
            {
                p = _p;
                n = _n;
            }

            public int CompareTo(State obj)
            {
                return p.CompareTo(obj);
            }
        }
        /*static unsafe class PQ
        {
            static State* s;
            public static int edx;
            static int* ixs;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Init(State* _s, int* _ixs)
            {
                s = _s;
                ixs = _ixs;
                edx = 1;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static void r_up(int idx)
            {
                int p = idx / 2;
                while (idx != 1)
                {
                    if (s[idx].p > s[p].p) return;
                    (s[idx], s[p]) = (s[p], s[idx]);
                    (ixs[s[idx].n], ixs[s[p].n]) = (idx, p);
                    idx = p;
                    p = idx / 2;
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static void r_down(int idx)
            {
                int nx = idx * 2;
                while (nx < edx)
                {
                    if (edx != nx + 1 && s[nx + 1].p < s[nx].p) nx++;
                    if (s[nx].p > s[idx].p) return;
                    (s[nx], s[idx]) = (s[idx], s[nx]);
                    (ixs[s[idx].n], ixs[s[nx].n]) = (idx, nx);
                    idx = nx;
                    nx = idx * 2;
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Push(int n, int p)
            {
                s[edx] = new State(p, n);
                ixs[n] = edx;
                r_up(edx);
                edx++;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Update(int n, int p)
            {
                int idx = ixs[n];
                int cp = s[idx].p;
                if (cp == p) return;
                s[idx].p = p;
                if (cp > p)
                {
                    r_up(idx);
                }
                else
                {
                    r_down(idx);
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int Pop(out int p)
            {
                State ret = s[1];
                s[1] = s[edx - 1];
                edx--;
                r_down(1);
                p = ret.p;
                ixs[ret.n] = 0;
                return ret.n;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool Contains(int n)
            {
                return (ixs[n] != 0);
            }
        }*/

        /* what about a fib heap */

        public static unsafe int Answer(int numOfServers, int targetServer, int[,] connectionTimeMatrix)
        {
            int sz = numOfServers + 10;
            var heap = HeapFactory.NewFibonacciHeap<State>();
            /*State* s = stackalloc State[sz];
            int* ixs = stackalloc int[sz];
            PQ.Init(s, ixs);*/
            bool* v = stackalloc bool[sz];
            int* sp = stackalloc int[sz];
            sp[0] = 0;
            for (int i = 1; i < numOfServers; i++) sp[i] = 1<<30;
            heap.Add(new State(0, 0));
            int cn, p;
            while (heap.Count > 0)
            {
                State f = heap.RemoveMin();
                cn = f.n;
                p = f.p;
                if (cn == targetServer) return p;
                if (v[cn]) continue;
                v[cn] = true;
                for (int i = 1; i < numOfServers; i++)
                {
                    if (v[i]) continue;
                    int np = sp[cn] + connectionTimeMatrix[cn, i];
                    if (np < sp[i])
                    {
                        heap.Add(new State(np, i));
                        sp[i] = np;
                    }
                }
            }
            return connectionTimeMatrix[0, targetServer];
        }
    }
}