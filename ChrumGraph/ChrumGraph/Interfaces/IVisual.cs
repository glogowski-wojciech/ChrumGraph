﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChrumGraph
{
    /// <summary>
    /// Interface of Visual for Core.
    /// </summary>
    public interface IVisual
    {
        /// <summary>
        /// Creates visual representation of a given Core vertex.
        /// </summary>
        /// <param name="vertex">Core vertex to be expanded.</param>
        void CreateVisualVertex(Vertex vertex);
        /// <summary>
        /// Removes visual representation of a given vertex.
        /// </summary>
        /// <param name="vertex">Vertex whose visual representation should be removed.</param>
        void RemoveVisualVertex(Vertex vertex);

        /// <summary>
        /// Creates visual representation of a given Core edge.
        /// </summary>
        /// <param name="edge">Core edge to be expanded.</param>
        void CreateVisualEdge(Edge edge);

        /// <summary>
        /// Removes visual representation of a given edge.
        /// </summary>
        /// <param name="edge">Edge whose visual representation should be removed.</param>
        void RemoveVisualEdge(Edge edge);

        /// <summary>
        /// Specifies whether graph should be visible for user.
        /// </summary>
        bool Visible { get; set; }

        /// <summary>
        /// Redraws graph. If graph was not visible, the Visible property is set to true.
        /// </summary>
        void Refresh();
    }
}
