using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms
{
    public class Edge
    {
        public int From { get; set; }
        public int To { get; set; }
        public Edge(int from, int to)
        {
            this.From = from;
            this.To = to;
        }
    }
    public class Vertice
    {
        public int Name { get; set; }
        public bool IsVisited { get; set; }
        public List<int> Adjacency { get; set; }
        public int Dist { get; set; }
        public Vertice prev { get; set; }
        public int Pre { get; set; }
        public int Post { get; set; }
        public Vertice(int name, bool visited)
        {
            this.Name = name;
            this.IsVisited = visited;
            this.Adjacency = new List<int>();
        }
        public void addAdjacency(int adj)
        {
            this.Adjacency.Add(adj);
        }
    }
    public class Graph
    {
        public Dictionary<int,Vertice> d { get; set; }
        public int CcNum { get; set; }
        public Graph(Edge[] edgesArr)
        {
            for (int i = 0; i < edgesArr.Length; i++)
            {
                if (!d.ContainsKey(edgesArr[i].From))
                    d.Add(edgesArr[i].From, new Vertice(edgesArr[i].From, false));
                if (!d.ContainsKey(edgesArr[i].To))
                    d.Add(edgesArr[i].To, new Vertice(edgesArr[i].To, false));
                d[edgesArr[i].From].addAdjacency(edgesArr[i].To);
                d[edgesArr[i].To].addAdjacency(edgesArr[i].From);
            }
        }

        public void Explore(Vertice v)
        {
            v.IsVisited = true;
            v.Pre = this.CcNum++;
            foreach (int adj in v.Adjacency)
            {
                if (!d[adj].IsVisited)
                    Explore(d[adj]);
            }
            v.Post = this.CcNum++;
        }

        public int DFS()
        {
            int cc = 0;
            this.CcNum = 0;
            foreach (var item in d)
            {
                if (!item.Value.IsVisited)
                {
                    Explore(item.Value);
                    cc++;
                }
            }
            return cc;
        }

        public void BFS(Vertice s)
        {
            foreach (var item in d.Values)
            {
                item.prev = null;
                item.Dist = int.MaxValue;
            }
            s.Dist = 0;
            Queue<Vertice> q = new Queue<Vertice>();
            q.Enqueue(s);
            Vertice u;
            while (q.Count != 0)
            {
                u = q.Dequeue();
                foreach (var item in u.Adjacency)
                {
                    if (d[item].Dist == int.MaxValue)
                    {
                        q.Enqueue(d[item]);
                        d[item].Dist = u.Dist + 1;
                        d[item].prev = u;
                    }
                }
            }
        }

        public Vertice[] TopologicalSort()
        {
            List<Vertice> lVer=new List<Vertice>();
            this.DFS();
            return this.MergePostSort();
        }

        public Vertice[] MergePostSort()
        {
           return MergePostSort(d.Values.ToArray(), 0, d.Count-1);

        }
        
        private Vertice[] MergePostSort(Vertice[] arr, int start, int end)
        {
            if (start != end)
            {
                MergePostSort(arr, start, (start + end) / 2);
                MergePostSort(arr, ((start + end) / 2) + 1, end);
                MergePost(arr, start, end);
            }
            return arr;
        }
        private void MergePost(Vertice[] arr, int start, int end)
        {
            int a = start, b = ((start + end) / 2) + 1, index = 0;
            Vertice[] temp = new Vertice[(end - start) + 1];
            while (index != (end - start) + 1)
            {
                if (arr[a].Post < arr[b].Post)
                {
                    temp[index++] = arr[a];
                    a++;
                    if (a == ((start + end) / 2) + 1)
                        while (b != end + 1)
                        {
                            temp[index++] = arr[b];
                            b++;
                        }
                }
                else
                {
                    temp[index++] = arr[b];
                    b++;
                    if (b == end + 1)
                        while (a != ((start + end) / 2) + 1)
                        {
                            temp[index++] = arr[a];
                            a++;
                        }
                }
            }
            for (int i = 0; i < temp.Length; i++)
            {
                arr[start + i] = temp[i];
            }
        }
    }
}