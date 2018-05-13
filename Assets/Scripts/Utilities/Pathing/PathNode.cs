using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode<T>{

	public T data; // node data
	public PathEdge<T>[] edges; //out nodes.

}
