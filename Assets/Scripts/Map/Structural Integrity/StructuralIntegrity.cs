using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class StructuralIntegrity : MonoBehaviour
{
    //     interface Edge {
    //     v: number,
    //     flow: number,
    //     C: number,
    //     rev: number
    // }

    private static int vertexCount;
    private static int source;
    private static int sink;
    private static int[] level;
    private static Dictionary<int, List<Edge>> adj = new Dictionary<int, List<Edge>>();

    public static void Setup(Map map)
    {
        vertexCount = 2 + map.width * map.height;
        source = vertexCount - 2;
        sink = vertexCount - 1;
        level = new int[vertexCount];
    }

    public static void Tick(Map map)
    {
        CreateEdges(map);

        foreach (Cell cell in map.solidCells)
        {
            cell.structurallySound = true;
        }

        List<int> bottleneckCells = BottleneckBFS(source, sink);
        foreach (int cellIndex in bottleneckCells)
        {
            Cell cell = map.GetCellByIndex(cellIndex);
            cell.structurallySound = false;
            Debug.Log(cell.position);
        }

        MarkFloating(map);
    }

    private static void MarkFloating(Map map)
    {
        HashSet<Cell> cells = new HashSet<Cell>(map.solidCells);

        while (cells.Count > 0)
        {
            Cell firstCell = cells.ElementAt(0);
            (HashSet<Cell> connected, bool grounded) = GetConnected(map, firstCell, new HashSet<Cell>());

            foreach (Cell cell in connected)
            {
                if (!grounded)
                    cell.structurallySound = false;

                cells.Remove(cell);
            }
        }
    }

    private static (HashSet<Cell>, bool) GetConnected(Map map, Cell cell, HashSet<Cell> result, bool hitBottom = false)
    {
        if (result == null)
            result = new HashSet<Cell>();

        if (cell == null || !cell.Solid || result.Contains(cell)) // Cell is null or solid or we've already checked it
            return (result, hitBottom);
        // Otherwise we are connected and solid
        result.Add(cell);

        if (!hitBottom && cell.position.y == 0)
            hitBottom = true;

        if (cell.position.y > 0)
        {
            (HashSet<Cell> _, bool newHitBottom) = GetConnected(map, map.GetCell(cell.x, cell.y - 1), result, hitBottom);
            hitBottom = hitBottom || newHitBottom;
        }
        if (cell.position.y < map.height - 1)
        {
            (HashSet<Cell> _, bool newHitBottom) = GetConnected(map, map.GetCell(cell.x, cell.y + 1), result, hitBottom);
            hitBottom = hitBottom || newHitBottom;
        }
        if (cell.position.x > 0)
        {
            (HashSet<Cell> _, bool newHitBottom) = GetConnected(map, map.GetCell(cell.x - 1, cell.y), result, hitBottom);
            hitBottom = hitBottom || newHitBottom;
        }
        if (cell.position.y < map.width - 1)
        {
            (HashSet<Cell> _, bool newHitBottom) = GetConnected(map, map.GetCell(cell.x + 1, cell.y), result, hitBottom);
            hitBottom = hitBottom || newHitBottom;
        }

        return (result, hitBottom);
    }

    private static void CreateEdges(Map map)
    {
        int xyToIndex(int x, int y) => x + y * map.width;
        adj.Clear();

        for (int i = 0; i < vertexCount; i++)
        {
            adj.Add(i, new List<Edge>());
        }

        // for (int y = 0; y < map.height; y++)
        // {
        //     for (int x = 0; x < map.width; x++)
        //     {

        for (int i = 0; i < map.solidCells.Count; i++)
        {
            Cell cell = map.solidCells[i];
            int x = cell.position.x;
            int y = cell.position.y;
            if (cell.position.y != 0)
                AddEdge(xyToIndex(x, y), xyToIndex(x, y - 1), cell.Strength);
            if (cell.position.y != map.height - 1)
                AddEdge(xyToIndex(x, y), xyToIndex(x, y + 1), cell.Strength);
            if (cell.position.x != 0)
                AddEdge(xyToIndex(x, y), xyToIndex(x - 1, y), cell.Strength);
            if (cell.position.x != map.width - 1)
                AddEdge(xyToIndex(x, y), xyToIndex(x + 1, y), cell.Strength);
            // source / sink
            AddEdge(source, xyToIndex(x, y), cell.Weight);
            if (y == 0)
                AddEdge(xyToIndex(x, y), sink, Mathf.Infinity);
        }
        //         }
        // }
    }

    private static void AddEdge(int from, int to, float maxFlow)
    {
        Edge a = new Edge { vertex = to, flow = 0, maxFlow = maxFlow, rev = adj[to].Count };
        Edge b = new Edge { vertex = from, flow = 0, maxFlow = 0, rev = adj[from].Count };
        adj[from].Add(a);
        adj[to].Add(b);
    }

    private static List<int> BottleneckBFS(int source, int sink)
    {
        float SendFlow(int u, float flow, int[] s, int t)
        {
            //     if (u === t) return flow;
            if (u == t) return flow;
            //     for (; s[u] < this.adj[u].length; s[u]++)
            while (s[u] < adj[u].Count)
            //     {
            {
                //         const edge = this.adj[u][s[u]];
                Edge edge = adj[u][s[u]];
                //         if (this.level[edge.v] === this.level[u] + 1 && edge.flow < edge.C)
                if (level[edge.vertex] == level[u] + 1 && edge.flow < edge.maxFlow)

                //         {
                {
                    //             let flowing = Math.min(flow, edge.C - edge.flow);
                    float flowing = Mathf.Min(flow, edge.maxFlow - edge.flow);
                    //             let temp_flow = this.sendFlow(edge.v, flowing, s, t)
                    float tempFlow = SendFlow(edge.vertex, flowing, s, t);
                    //           if (temp_flow > 0)
                    if (tempFlow > 0)
                    //             {
                    {
                        //                 edge.flow += temp_flow
                        edge.flow += tempFlow;
                        //     this.adj[edge.v][edge.rev].flow -= temp_flow
                        Edge oldEdge = adj[edge.vertex][edge.rev];
                        Edge newEdge = new Edge { vertex = oldEdge.vertex, flow = oldEdge.flow - tempFlow, maxFlow = oldEdge.maxFlow, rev = oldEdge.rev };
                        // adj[edge.vertex][edge.rev].flow -= tempFlow;
                        adj[edge.vertex][edge.rev] = newEdge;
                        // Edge newEdge = new Edge { vertex: edge.vertex }
                        // var test = adj[edge.vertex];
                        // test[edge.rev].flow -= tempFlow;
                        //     return temp_flow
                        return tempFlow;
                        //   }
                        //         }
                    }
                    //     }
                }
                s[u]++;
            }
            //     return 0
            return 0;
        }

        bool LevelGraphsBFS()
        {
            //         const level = Array(this.V).fill(-1)
            for (int i = 0; i < vertexCount; i++)
            {
                level[i] = -1;
            }
            //   level[this.source()] = 0
            level[source] = 0;
            //   // BFS
            //   const queue = [s]
            Queue<int> queue = new Queue<int>();
            queue.Enqueue(source);
            //   while (queue.length)
            while (queue.Count > 0)
            //         {
            {
                //             const u = queue.shift()
                int u = queue.Dequeue();
                //     if (u === undefined) continue;
                //             for (const edge of this.adj[u]) {
                foreach (Edge edge in adj[u])
                {
                    //         if (level[edge.v] < 0 && edge.flow < edge.C)
                    if (level[edge.vertex] < 0 && edge.flow < edge.maxFlow)
                    //         {
                    {
                        //             level[edge.v] = level[u] + 1;
                        level[edge.vertex] = level[u] + 1;
                        //             queue.push(edge.v)
                        queue.Enqueue(edge.vertex);
                        //         }
                    }
                    //     }
                }
                // }
            }
            //   //
            //   this.level = level
            //   return this.level[this.sink()] !== -1
            return level[sink] != -1;
        }

        //           if (s===t) return -1;
        float total = 0;

        //   let total = 0
        //   while (this.levelGraphBFS(s, t)) {
        while (LevelGraphsBFS())
        {
            //     let sources = Array(this.V+1).fill(0)
            int[] sources = new int[vertexCount + 1];
            //     while (true) {
            while (true)
            {
                //       let flow = this.sendFlow(s, Infinity, sources, t)
                float flow = SendFlow(source, Mathf.Infinity, sources, sink);
                //       if (!flow) break;
                if (flow > 0) break;
                //       total += flow
                total += flow;
                //     }
            }
            //   }
        }
        //   return total;

        // const offending = []
        List<int> result = new List<int>();
        // const level = Array(this.vertexCount).fill(-1)
        // level[this.source()] = 0
        // level = new int[vertexCount];
        for (int i = 0; i < vertexCount; i++)
        {
            level[i] = -1;
        }
        // const queue = [s]
        // List<int> queue = new List<int>() {source};
        Queue<int> queue = new Queue<int>();
        queue.Enqueue(source);
        // while (queue.length) {
        while (queue.Count > 0)
        {
            //     const u = queue.shift()
            int u = queue.Dequeue();
            //     if (u === undefined) continue;
            //     // if all edges go to offending node, ignore current node
            //     let isBottleNeck = false
            bool isBottleNeck = false;
            //     for (const edge of this.adj[u]) {
            foreach (Edge edge in adj[u])
            {
                //         if (level[edge.v] < 0) {
                if (level[edge.vertex] < 0)
                {
                    //             if (edge.flow < edge.C) {
                    if (edge.flow < edge.maxFlow)
                    {
                        Debug.Log(edge.flow);
                        Debug.Log(edge.maxFlow);
                        //                 level[edge.v] = level[u] + 1;
                        level[edge.vertex] = level[u] + 1;
                        //                 queue.push(edge.v)
                        queue.Enqueue(edge.vertex);
                        //             } else if (edge.C && u !== t && u !== s) {
                    }
                    else if (edge.maxFlow > 0 && u != source && u != sink)
                    {
                        //                 isBottleNeck = true
                        isBottleNeck = true;
                        //             }
                    }
                    //         }
                }
                //     }
            }
            //     if (isBottleNeck) offending.push(u)
            if (isBottleNeck) result.Add(u);
            // }
        }
        // return offending
        return result;
    }
}
