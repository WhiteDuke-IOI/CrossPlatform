#include <limits.h>
#include <stdio.h>
#include <stdlib.h>

#ifndef FORD_BELLMAN
#define FORD_BELLMAN

struct Edge {
    int src, dest, weight;
};

struct Graph {
    int V, E;
    struct Edge* edge;
};

struct Graph* createGraph(int V, int E)
{
    struct Graph* graph
        = (struct Graph*)malloc(sizeof(struct Graph));
    graph->V = V;
    graph->E = E;
    graph->edge = (struct Edge*)malloc(
        graph->E * sizeof(struct Edge));
    return graph;
}

void BellmanFord(struct Graph* graph, int source[], int src, int end)
{
    int V = graph->V;
    int E = graph->E;
    int dist[V];
    int shr[V];

    for (int i = 0; i < V; i++)
        dist[i] = INT_MAX;
    dist[src] = 0;
    shr[src] = src;

    for (int i = 1; i <= V - 1; i++) {
        for (int j = 0; j < E; j++) {
            int u = graph->edge[j].src;
            int v = graph->edge[j].dest;
            int weight = graph->edge[j].weight;
            if (dist[u] != INT_MAX
                && dist[u] + weight < dist[v])
                {
                    dist[v] = dist[u] + weight;
                    shr[v] = u;
                }

        }
    }

    for (int i = 0; i < E; i++) {
        int u = graph->edge[i].src;
        int v = graph->edge[i].dest;
        int weight = graph->edge[i].weight;
        if (dist[u] != INT_MAX
            && dist[u] + weight < dist[v]) {
            source[0] = -1;
            //printf("Graph contains negative weight cycle");
            return;
        }
    }

    source[0] = 0;
    source[1] = dist[end];
    //printf("%d -> ", end);
    source[2] = end;
    int k = shr[end];
    source[3] = k;
    //printf("%d -> ", k);

    for (int i = 4;;i++)
    {
         if (k == shr[k])
         {
            source[i]=-1;
            break;
          }

         k = shr[k];
         source[i]=k;
         printf("%d ->", k);
    }

    //printf("Vertex   Distance from Source\n");
    //for (int i = 0; i < V; i++)
        //printf("%d \t\t %d\n", i, dist[i]);
    //printf("\n\n");

    //for (int i = 0; i < V; i++)
        //printf("%d \t\t %d\n", i, shr[i]);
}


void ford_bellman(int Vertex, int Edges, int arrayEdges[], int First, int End)
{
    struct Graph* graph = createGraph(Vertex, Edges);

    for (int i = 0; i < Edges; i++)
    {
      graph->edge[i].src = arrayEdges[3*i];
      graph->edge[i].dest = arrayEdges[3*i+1];
      graph->edge[i].weight = arrayEdges[3*i+2];
    }

    BellmanFord(graph, arrayEdges, First, End);
}
#endif
