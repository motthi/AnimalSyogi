using System;
using System.Drawing;
using System.Windows.Forms;

namespace AnimalSyogi {
    public partial class Form1 : Form {
        Board board = new Board();
        Koma chick_1;
        Koma chick_2;
        Koma giraffe_1;
        Koma giraffe_2;
        Koma elephant_1;
        Koma elephant_2;
        Koma lion_1;
        Koma lion_2;
        Point pos_init;
        int[,] chick_move = new int[3,3] { { 0, 1, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
        int[,] giraffe_move = new int[3, 3] { { 0, 1, 0 }, { 1, 0, 1 }, { 0, 1, 0 } };
        int[,] elephant_move = new int[3, 3] { { 1, 0, 1 }, { 0, 0, 0 }, { 1, 0, 1 } };
        int[,] lion_move = new int[3, 3] { { 1, 1, 1 }, { 1, 0, 1 }, { 1, 1, 1 } };
        int[,] chicken_move = new int[3, 3] { { 1, 1, 1 }, { 1, 0, 1 }, { 0, 1, 0 } };


        public Form1() {
            InitializeComponent();
            chick_1 = new Koma(pictureBox8, 0, 1, 2, chick_move);
            chick_2 = new Koma(pictureBox4, 1, 1, 1, chick_move);
            giraffe_1 = new Koma(pictureBox5, 0, 2, 3, giraffe_move);
            giraffe_2 = new Koma(pictureBox1, 1, 0, 0, giraffe_move);
            elephant_1 = new Koma(pictureBox7, 0, 0, 3, elephant_move);
            elephant_2 = new Koma(pictureBox3, 1, 2, 0, elephant_move);
            lion_1 = new Koma(pictureBox6, 0, 1, 3, lion_move);
            lion_2 = new Koma(pictureBox2, 1, 1, 0, lion_move);

            animal_Initialize();
        }

        private void resetButton_Click(object sender, EventArgs e) {
            chick_1.pos_x = 1;
            chick_1.pos_y = 2;
            chick_2.pos_x = 1;
            chick_2.pos_y = 1;
            giraffe_1.pos_x = 2;
            giraffe_1.pos_y = 3;
            giraffe_2.pos_x = 0;
            giraffe_2.pos_y = 0;
            elephant_1.pos_x = 0;
            elephant_1.pos_y = 3;
            elephant_2.pos_x = 2;
            elephant_2.pos_y = 0;
            lion_1.pos_x = 1;
            lion_1.pos_y = 3;
            lion_2.pos_x = 1;
            lion_2.pos_y = 0;
            animal_Initialize();
        }

        private void resetKoma(Koma koma, int forward) {
            tableLayoutPanel1.Controls.Add(koma.pictureBox, koma.pos_x, koma.pos_y);
            board.append(koma);

            /* 駒の向きの初期化 */
            if (koma.forward != forward) {
                koma.forward = forward;
                koma.pictureBox.Image.RotateFlip(RotateFlipType.RotateNoneFlipY);
            }

            /* 駒の所有者の初期化 */
            if (forward == 0) {
                koma.player = 0;
            } else if (forward == 1) {
                koma.player = 1;
            }
        }

        private void animal_Initialize() {
            resetKoma(chick_1, 0);
            resetKoma(chick_2, 1);
            resetKoma(giraffe_1, 0);
            resetKoma(giraffe_2, 1);
            resetKoma(elephant_1, 0);
            resetKoma(elephant_2, 1);
            resetKoma(lion_1, 0);
            resetKoma(lion_2, 1);
            board.player = 0;
        }

        private void picture_MouseUp(object sender, MouseEventArgs e) {
            Point pos = PointToClient(Cursor.Position);
            int row = 0;
            int column = 0;
            int row_init = 0;
            int column_init = 0;
            double distance = 0.0;
            Koma koma_init;
            Koma koma_del;
            Control pic_del;

            if (pos_init.X > 3 && pos_init.X < 95) {
                column_init = 0;
            } else if (pos_init.X < 195) {
                column_init = 1;
            } else if (pos_init.X < 290) {
                column_init = 2;
            }

            if (pos_init.Y > 10 && pos_init.Y < 96) {
                row_init = 0;
            } else if (pos_init.Y < 184) {
                row_init = 1;
            } else if (pos_init.Y < 270) {
                row_init = 2;
            } else if (pos_init.Y < 358) {
                row_init = 3;
            }

            if (pos.X > 3 && pos.X < 95) {
                column = 0;
            } else if (pos.X < 195) {
                column = 1;
            } else if (pos.X < 290) {
                column = 2;
            }

            if (pos.Y > 10 && pos.Y < 96) {
                row = 0;
            } else if (pos.Y < 184) {
                row = 1;
            } else if (pos.Y < 270) {
                row = 2;
            } else if (pos.Y < 358) {
                row = 3;
            }
            koma_init = board.get(column_init, row_init);   //移動元の駒

            /* プレーヤの手番を確認 */
            if (koma_init.player != board.player) {
                errorLabel.Text = "あなたの駒ではありません．";
                return;
            }

            /* 隣接していないマスには進めない */
            distance = Math.Sqrt(Math.Pow(row - row_init, 2) + Math.Pow(column - column_init, 2));
            if(distance > Math.Sqrt(2) + 1e-100) {
                errorLabel.Text = "駒の動きが間違っています．";
                return;
            }else if(distance < 1e-100) {
                return;
            }

            /* 駒が移動できるマスでない場合は移動しない */
            if (koma_init.forward == 0) {   //駒の向きで場合分け
                if (koma_init.move[row - row_init + 1, column - column_init + 1] == 0) {
                    errorLabel.Text = "駒の動きが間違っています．";
                    return;
                }
            } else {
                if (koma_init.move[row_init - row + 1, column_init - column + 1] == 0) {
                    errorLabel.Text = "駒の動きが間違っています．";
                    return;
                }
            }

            /* 相手の駒が移動先にある場合の処理 */
            pic_del = tableLayoutPanel1.GetControlFromPosition(column, row);
            koma_del = board.get(column, row);
            if (koma_del != null) {
                if (koma_init.player == koma_del.player) {
                    return;
                } else {
                    tableLayoutPanel1.Controls.Remove(pic_del);
                    if (koma_init.player == 1) {
                        koma_del.player = 1;    //駒の所有者を変える
                        tableLayoutPanel2.Controls.Add(koma_del.pictureBox, 0, 0);
                    } else if (koma_init.player == 0) {
                        koma_del.player = 0;    //駒の所有者を変える
                        tableLayoutPanel3.Controls.Add(koma_del.pictureBox, 0, 0);
                        koma_del.pictureBox.Image.RotateFlip(RotateFlipType.RotateNoneFlipY);
                        koma_del.forward = 0;
                    }
                    board.delete(koma_del);
                }
            }

            board.delete(koma_init);     //元いた場所の登録は削除
            koma_init.pos_x = column;    //盤上の位置を更新
            koma_init.pos_y = row;       //盤上の位置を更新
            board.append(koma_init);     //新しい盤上の位置に登録
            tableLayoutPanel1.Controls.Add(koma_init.pictureBox, column, row);   //描画
            board.player = (board.player + 1) % 2;  //プレイヤーの切り替え
            errorLabel.Text = "";
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                pos_init = PointToClient(Cursor.Position);
            }
        }

        public class Board {
            public Koma[,] board = new Koma[3, 4];
            public int player = 0;

            public void append(Koma koma) {
                this.board[koma.pos_x, koma.pos_y] = koma;
            }

            public void delete(Koma koma) {
                this.board[koma.pos_x, koma.pos_y] = null;
            }

            public Koma get(int x, int y) {
                return this.board[x, y];
            }
        }

        public class Koma {
            public int head = 0;    //駒の表裏，表：0，裏：1
            public int pos_x;
            public int pos_y;
            public int player;  //駒の所有者，手前：0，裏：1
            public int forward = 0; //駒の向き，上：0，下：1
            public int[,] move = new int[3, 3];
            public System.Windows.Forms.PictureBox pictureBox;

            public Koma(System.Windows.Forms.PictureBox pictureBox, int player, int pos_x, int pos_y, int[,] move) {
                this.player = player;
                this.pictureBox = pictureBox;
                this.pictureBox.Visible = true;
                this.pos_x = pos_x;
                this.pos_y = pos_y;
                this.move = move;
                if (this.player == 1) {
                    this.pictureBox.Image.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    this.pictureBox.Refresh();
                    this.forward = 1;
                }
            }
        }
    }
}
