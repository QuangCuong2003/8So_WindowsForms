using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _8So_WindowsForms
{
    public class Node
    {
        
        public int[,] matrix;// ma trận 8 số
        public int countWrongNumber;//số mảnh sai vị trí của ma trận
        public int value;// chỉ số của node
        public int parent;// cha của node, để truy vét kết quả
        public int feeNode;// chi phí đi đến node đó
    }
    public class Solution8Numbers
    {
        private int value = 0;//chỉ số của Node sẽ tăng sau mỗi lần sinh ra 1 node
        private int feeNode = 0;// Sau mỗi lần sinh ra các node thì chi phí các node tăng 1 đơn vị, tức node con sẽ có chi phí lớn hơn node cha 1 đơn vị
        public Stack<int[,]> findResult(int[,] MaTran)
        {
            List<Node> close = new List<Node>();
            List<Node> open = new List<Node>();

            //khai báo và khởi tạo cho node đầu tiên
            Node tSo = new Node();
            tSo.matrix = MaTran;
            tSo.countWrongNumber = getCountNumbersWrong(MaTran);
            tSo.value = 0;
            tSo.parent = -1;
            tSo.feeNode = 0;
            //cho trạng thái đầu tiên vào Open;
            open.Add(tSo);

            int t = 0;
            while(open.Count!=0)
            {
                tSo = open[t];
                open.Remove(tSo);
                close.Add(tSo);

                //nếu node có số mảnh sai là 0, tức là đích thì thoát
                if (tSo.countWrongNumber == 0) break;
                else
                {
                    //sinh hướng đi của node hiện tại
                    List<Node> lstDirection = createDirection(tSo);

                    for (int i = 0; i < lstDirection.Count; i++)
                    {
                        //hướng đi không thuộc Open và Close
                        if (!sameNode(lstDirection[i], open) && !sameNode(lstDirection[i], close))
                        {
                            open.Add(lstDirection[i]);
                        }
                        else
                        {   //nếu hướng đi thuộc Open
                            if (sameNode(lstDirection[i], open))
                            {
                                /*nếu hướng đi đó tốt hơn thì sẽ được cập nhật lại, 
                                ngược lại thì sẽ không cập nhật*/
                                compareBetter(lstDirection[i], open);
                            }
                            else
                            {
                                //nếu hướng đi thuộc Close
                                if (sameNode(lstDirection[i], close))
                                {
                                    /*nếu hướng đi đó tốt hơn thì sẽ được cập nhật lại, 
                                    ngược lại thì sẽ không cập nhật và chuyển từ Close sang Open*/
                                    if (compareBetter(lstDirection[i], close))
                                    {
                                        Node temp = getSameNodeInClose(lstDirection[i], close);
                                        close.Remove(temp);
                                        open.Add(temp);
                                    }
                                }
                            }
                        }

                    }

                    //chọn vị trí có phí tốt nhất trong Open
                    t = betterOpen(open);
                }

            }

            //truy vét kết quả tỏng tập Close
            return getResult(close);
        }

        //truy vét kết quả đường đi trong tập Close
        Stack<int[,]> getResult(List<Node> close)
        {
            Stack<int[,]> rsult = new Stack<int[,]>();

            int t = close[close.Count - 1].parent;
            Node temp = new Node();
            rsult.Push(close[close.Count - 1].matrix);

            while (t != -1)
            {
                for (int i = 0; i < close.Count; i++)
                {
                    if (t == close[i].value)
                    {
                        temp = close[i];
                        break;
                    }
                }

                rsult.Push(temp.matrix);
                t = temp.parent;
            }

            return rsult;
        }

        List<Node> createDirection(Node tSo)
        {
            int n = tSo.matrix.GetLength(0);//lấy số hàng của ma trận

            List<Node> lstDirection = new List<Node>();

            int h;
            int c = 0;
            bool ok = false;
            for (h = 0; h < n; h++)
            {
                for (c = 0; c < n; c++)
                    if (tSo.matrix[h, c] == 0)
                    {
                        ok = true;
                        break;
                    }

                if (ok) break;
            }

            Node Temp = new Node();
            Temp.matrix = new int[n, n];
            //Copy mảng Ma trận sang mảng ma trận tạm
            Array.Copy(tSo.matrix, Temp.matrix, tSo.matrix.Length);

            feeNode++;// tăng chi phí của node con lên 1 đơn vị

            //xét hàng ngang bắt đầu từ hàng thứ 2 trở đi
            if (h > 0 && h <= n - 1)
            {
                // thay đổi hướng đi của ma trận
                Temp.matrix[h, c] = Temp.matrix[h - 1, c];
                Temp.matrix[h - 1, c] = 0;

                //cập nhật lại thông số của node
                Temp.countWrongNumber = getCountNumbersWrong(Temp.matrix);
                value++;
                Temp.value = value;
                Temp.parent = tSo.value;
                Temp.feeNode = feeNode + Temp.countWrongNumber;
                lstDirection.Add(Temp);

                //sau khi thay đổi ma trận thì copy lại ma trận cha cho MaTran để xét trường hợp tiếp theo
                Temp = new Node();
                Temp.matrix = new int[n, n];
                Array.Copy(tSo.matrix, Temp.matrix, tSo.matrix.Length);
            }
            //xét hàng ngang bắt đầu từ hàng thứ cuối cùng - 1 trở xuống
            if (h < n - 1 && h >= 0)
            {
                // thay đổi hướng đi của ma trận
                Temp.matrix[h, c] = Temp.matrix[h + 1, c];
                Temp.matrix[h + 1, c] = 0;

                //cập nhật lại thông số của node
                Temp.countWrongNumber = getCountNumbersWrong(Temp.matrix);
                value++;
                Temp.value = value;
                Temp.parent = tSo.value;
                Temp.feeNode = feeNode + Temp.countWrongNumber;
                lstDirection.Add(Temp);

                //sau khi thay đổi ma trận thì copy lại ma trận cha cho MaTran để xét trường hợp tiếp theo
                Temp = new Node();
                Temp.matrix = new int[n, n];
                Array.Copy(tSo.matrix, Temp.matrix, tSo.matrix.Length);
            }
            //Xét cột dọc bắt đầu từ cột thứ 2 trở đi
            if (c > 0 && c <= n - 1)
            {
                // thay đổi hướng đi của ma trận
                Temp.matrix[h, c] = Temp.matrix[h, c - 1];
                Temp.matrix[h, c - 1] = 0;

                //cập nhật lại thông số của node
                Temp.countWrongNumber = getCountNumbersWrong(Temp.matrix);
                value++;
                Temp.value = value;
                Temp.parent = tSo.value;
                Temp.feeNode = feeNode + Temp.countWrongNumber;
                lstDirection.Add(Temp);

                //sau khi thay đổi ma trận thì copy lại ma trận cha cho MaTran để xét trường hợp tiếp theo
                Temp = new Node();
                Temp.matrix = new int[n, n];
                Array.Copy(tSo.matrix, Temp.matrix, tSo.matrix.Length);
            }
            //Xét cột dọc bắt đầu từ cột cuối cùng -1 trở xuống
            if (c < n - 1 && c >= 0)
            {
                // thay đổi hướng đi của ma trận
                Temp.matrix[h, c] = Temp.matrix[h, c + 1];
                Temp.matrix[h, c + 1] = 0;

                //cập nhật lại thông số của node
                Temp.countWrongNumber = getCountNumbersWrong(Temp.matrix);
                value++;
                Temp.value = value;
                Temp.parent = tSo.value;
                Temp.feeNode = feeNode + Temp.countWrongNumber;
                lstDirection.Add(Temp);

                //đến đây đã xết hết hướng đi nên không cần copy lại ma trận
            }

            return lstDirection;
        }

        
        int betterOpen(List<Node> Open)
        {
            if (Open.Count != 0)
            {
                Node min;
                min = Open[0];
                int vt = 0;

                for (int i = 1; i < Open.Count; i++)
                    if (min.countWrongNumber > Open[i].countWrongNumber)
                    {
                        min = Open[i];
                        vt = i;
                    }
                    else    
                    {
                        if (min.countWrongNumber == Open[i].countWrongNumber)
                        {
                            if (min.feeNode > Open[i].feeNode)
                            {
                                min = Open[i];
                                vt = i;
                            }
                        }
                    }
                return vt;
            }

            return 0;
        }


        /// <summary>
        /// So sánh chi phí của hai node
        /// </summary>
        /// <param name="TamSo">Node cần so sánh</param>
        /// <param name="lst8So">Tập Open hoặc Close</param>
        /// <returns>trả về true nếu tốt hơn và cập nhật lại cha và chi phí cho node, ngược lại không làm gì và trả về false </returns>
        bool compareBetter(Node tSo, List<Node> lst8So)
        {
            for (int i = 0; i < lst8So.Count; i++)
                if (equalMatrix(tSo.matrix, lst8So[i].matrix))
                {
                    if (tSo.feeNode < lst8So[i].feeNode)
                    {
                        //vì 2 ma trận bằng nhau lên số mảnh sai vi trị là như nhau lên ta không cần cập nhật
                        lst8So[i].parent = tSo.parent;// cập nhật lại cha của hướng đi
                        lst8So[i].feeNode = tSo.feeNode;// cập nhật lại chi phí đường đi

                        return true;
                    }
                    else return false;
                }

            return false;
        }


        /// <summary>
        /// Lấy ra node bị trùng trong tập Close
        /// </summary>
        /// <param name="TamSo">node trùng</param>
        /// <param name="lst8So">tập Close</param>
        /// <returns>Trả về node bị trùng</returns>
        Node getSameNodeInClose(Node tSo, List<Node> lst8Numbers)
        {
            Node Trung = new Node();
            for (int i = 0; i < lst8Numbers.Count; i++)
                if (equalMatrix(tSo.matrix, lst8Numbers[i].matrix))
                {
                    Trung = lst8Numbers[i];
                    break;
                }
            return Trung;
        }


        /// <summary>
        /// So sánh node này có trùng với 1 node trong danh sách các node khác không
        /// </summary>
        /// <param name="TamSo">node cần so sánh</param>
        /// <param name="lst8So">dánh sách các node cần so sánh</param>
        /// <returns>Trả về true nếu trùng, ngược lại trả về false </returns>
        bool sameNode(Node tSo, List<Node> lst8So)
        {
            for (int i = 0; i < lst8So.Count; i++)
                if (equalMatrix(lst8So[i].matrix, tSo.matrix))
                    return true;

            return false;
        }


        /// <summary>
        /// So sánh hai ma trận có các phần tử bằng nhau hay không
        /// </summary>
        /// <param name="a">Ma trận 1</param>
        /// <param name="b">Ma trận 2</param>
        /// <returns>Trả về true nếu bằng nhau ngược lại trả về false</returns>
        bool equalMatrix(int[,] a, int[,] b)
        {
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(0); j++)
                    if (a[i, j] != b[i, j])
                        return false;
            }

            return true;
        }


        /// <summary>
        /// trả về số mảnh nằm sai vị trí
        /// </summary>
        /// <param name="MaTran">Ma trận</param>
        /// <returns>Trả về số miếng sai vị trí</returns>
        int getCountNumbersWrong(int[,] maxtrix)
        {
            int dung = 0;
            int t = 0;
            for (int i = 0; i < maxtrix.GetLength(0); i++)
            {
                if (i == 0)
                    for (int j = 0; j < maxtrix.GetLength(0); j++)
                    {
                        t++;
                        if (maxtrix[i, j] == t)
                            dung++;
                    }
                else
                {
                    if (maxtrix[1, 2] == 4)
                        dung++;
                    if (maxtrix[2, 2] == 5)
                        dung++;
                    if (maxtrix[2, 1] == 6)
                        dung++;
                    if (maxtrix[2, 0] == 7)
                        dung++;
                    if (maxtrix[1, 0] == 8)
                        dung++;

                    break;
                }
            }
            return 8-dung;
        }

        // sinh một ma trận ngẫu nhiên để làm node bắt đầu
        public int[,] randomMatrix(int kickThuoc)
        {
            
            int[,] matrix = new int[kickThuoc, kickThuoc];
            //khởi tạo ma trận 8 số
            matrix[0, 0] = 1;
            matrix[0, 1] = 2;
            matrix[0, 2] = 3;
            matrix[1, 0] = 8;
            matrix[1, 1] = 0;
            matrix[1, 2] = 4;
            matrix[2, 0] = 7;
            matrix[2, 1] = 6;
            matrix[2, 2] = 5;


            //tập Close lưu lại các hướng đã đi để đảm bảo sinh ra hướng đi mới không trùng lặp
            List<int[,]> close = new List<int[,]>();

            int n = matrix.GetLength(0);

            int[,] temp = new int[n,n];
            Array.Copy(matrix, temp, matrix.Length);
            close.Add(temp);
            int h = 1, c = 1;

            Random rd = new Random();

            int m = rd.Next(10, 200);//lấy số lần lặp sinh hướng đi
            int t = rd.Next(1, 5);// t=[1...4] tương ứng với 4 hướng đi

            //số lần lặp được lấy random từ đó số lượng hướng đi sẽ thay đổi theo
            for (int r = 0; r < m; r++)
            {
                // vì t được lấy random nên hướng đi sẽ ngẫu nhiên, có thể lên, xuống, trái, phải tùy vào biến t

                //đi lên trên với t =1
                if (h > 0 && h <= n - 1&&t==1)
                {
                    matrix[h, c] = matrix[h - 1, c];
                    matrix[h - 1, c] = 0;
                    
                    if (!hasMatrix(matrix, close))
                    {
                        h--;
                        temp = new int[n, n];
                        Array.Copy(matrix, temp, matrix.Length);
                        close.Add(temp);
                    }
                    else
                    {
                        matrix[h - 1, c] = matrix[h, c];
                        matrix[h, c] = 0;
                    }
       
                }

                t = rd.Next(1, 5);

                //đi sang trái với t=2
                if (c > 0 && c <= n - 1&&t==2)
                {
                    matrix[h, c] = matrix[h, c - 1];
                    matrix[h, c - 1] = 0;
                    
                    if (!hasMatrix(matrix, close))
                    {
                        c--;
                        temp = new int[n, n];
                        Array.Copy(matrix, temp, matrix.Length);
                        close.Add(temp);
                    }
                    else
                    {
                        matrix[h, c - 1] = matrix[h, c];
                        matrix[h, c] = 0;
                    }
                }

                t = rd.Next(1, 5);

                //đi xuống giưới với t=3
                if (h < n - 1 && h >= 0&&t==3)
                {
                    matrix[h, c] = matrix[h + 1, c];
                    matrix[h + 1, c] = 0;

                    if (!hasMatrix(matrix, close))
                    {
                        h++;
                        temp = new int[n, n];
                        Array.Copy(matrix, temp, matrix.Length);
                        close.Add(temp);
                    }
                    else
                    {
                        matrix[h + 1, c] = matrix[h, c];
                        matrix[h, c] = 0;
                    }

                }

                t = rd.Next(1, 5);

                //đi sang phải với t = 4
                if (c < n - 1 && c >= 0&&t==4)
                {
                    matrix[h, c] = matrix[h, c + 1];
                    matrix[h, c + 1] = 0;
                    
                    if (!hasMatrix(matrix, close))
                    {
                        c++;
                        temp = new int[n, n];
                        Array.Copy(matrix, temp, matrix.Length);
                        close.Add(temp);
                    }
                    else
                    {
                        matrix[h, c + 1] = matrix[h, c];
                        matrix[h, c] = 0;
                    }
                }

            }

            // trả về hướng đi cuối dùng trong danh sách hướng đi
            return close[close.Count-1];
        }
        
        //So sánh nếu ma trận a đã có trang danh sách Close  thì trả về true ngược lại trả về false
        bool hasMatrix(int[,] a, List<int[,]> Close)
        {
            for(int i=0;i<Close.Count;i++)
            {
                if(equalMatrix(a,Close[i]))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
