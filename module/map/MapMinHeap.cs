using System.Collections;
using System.Collections.Generic;


public class HeapNode
{
    public int X;
    public int Y;
    public float G;
    public float H;
    public HeapNode Parent;
    //next only use when route has been found
    public HeapNode Next;
}


public class MapMinHeap {

    private List<HeapNode> heap;
    private int count;

    public int Count(){
        return count;
    }


    public MapMinHeap(){
        heap = new List<HeapNode>();
        count = 0;
    }

    public bool Find(int x,int y,float g,float h,HeapNode curr){
        int index = heap.FindIndex(node => node.X == x && node.Y == y);
        if(index == -1){
            Push(x, y, g, h, curr);
            return false;
        }else{
            Replace(index, g,h,curr);
            return true;
        }
    }
    

    public void Push(int x,int y,float g,float h,HeapNode curr){
        if (x < 0 || x >= BattleDef.columnGridNum || y < 0 || y >= BattleDef.rowGridNum) return;
        count = count + 1;
        HeapNode node = new HeapNode
        {
            X = x,
            Y = y,
            G = g,
            H = h,
            Parent = curr,
        };
        heap.Add(node);
        ShiftUp(count - 1);
    }

    private void Replace(int index,float g,float h,HeapNode curr){
        if ((heap[index].G+heap[index].H) <= (g+h)) return;
        HeapNode newNode = new HeapNode
        {
            X = heap[index].X,
            Y = heap[index].Y,
            G = g,
            H = h,
            Parent = curr
        };
        heap[index] = newNode;
        ShiftUp(index);
    }

    public HeapNode Pop(){
        HeapNode res = heap[0];
        heap[0] = heap[count - 1];
        heap.RemoveAt(count - 1);
        count = count - 1;
        ShiftDown(0);
        return res;
    }

    private void ShiftUp(int index){
        if (index <= 0 || index >= count) return;
        int parentIndex = (index - 1) / 2;
        while(index > 0){
            if((heap[parentIndex].G+heap[parentIndex].H)<=(heap[index].G+heap[index].H)){
                break;
            }else{
                HeapNode tem = heap[parentIndex];
                heap[parentIndex] = heap[index];
                heap[index] = tem;
            }
            index = parentIndex;
            parentIndex = (index - 1) / 2;
        }
    }

    private void ShiftDown(int index){
        int lChild = index * 2 + 1;
        int rChild = index * 2 + 2;
        while(lChild<count||rChild<count){
            int minIndex = index;
            if(lChild<count && (heap[lChild].G+heap[lChild].H)<(heap[minIndex].G+heap[minIndex].H)){
                minIndex = lChild;
            }
            if(rChild<count && (heap[rChild].G+heap[rChild].H)<(heap[minIndex].G+heap[minIndex].H)){
                minIndex = rChild;
            }
            if(minIndex == index){
                break;
            }else{
                HeapNode tem = heap[index];
                heap[index] = heap[minIndex];
                heap[minIndex] = tem;
            }
            index = minIndex;
            lChild = index * 2 + 1;
            rChild = index * 2 + 2;
        }
    }
}
