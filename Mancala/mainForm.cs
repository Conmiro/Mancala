using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mancala
{
    public partial class mainForm : Form
    {

        //                    {S1, A1, A2, A3, A4, A5, A6, S2, B1, B2, B3, B4, B5, B6}
        //                    { 0,  1,  2,  3,  4,  5,  6,  7,  8,  9, 10, 11, 12, 13}
        private int[] cells = {0, 4, 4, 4, 4, 4, 4, 0, 4, 4, 4, 4, 4, 4};
        private List<int[]> previousCells = new List<int[]>();
        private int currentPrevIndex = 0;

        private int pTurn = 1; //Current player's turn..
        private bool gameOver = false; //Is the game over?
        private int amt; //Used to see how many pebbles in the cell clicked.
        private int currCell; //Used to track the current cell.
        private List<Button> buttons = new List<Button>();


        private void newGame()
        {
            for (int i = 0; i <= 13;i++ )
            {
                if (i != 0 && i != 7)
                    cells[i] = 4;
                else
                    cells[i] = 0;
            }
            pTurn = 1;
            gameOver = false;
            updateButtons();
        }

        //Creates a list with the buttons with indexes that match the array.
        private void createList()
        {
            buttons.Add(this.S1);
            buttons.Add(this.A1);
            buttons.Add(this.A2);
            buttons.Add(this.A3);
            buttons.Add(this.A4);
            buttons.Add(this.A5);
            buttons.Add(this.A6);
            buttons.Add(this.S2);
            buttons.Add(this.B1);
            buttons.Add(this.B2);
            buttons.Add(this.B3);
            buttons.Add(this.B4);
            buttons.Add(this.B5);
            buttons.Add(this.B6);
            updateButtons();
        }

        public mainForm()
        {
            InitializeComponent();
           
        }

        private void updateButtons()
        {
            for (int i = 0; i < buttons.Count; i++)
                buttons.ElementAt(i).Text = cells[i].ToString();


            if (pTurn == 1)
                this.turnLabel.Text = "Player 1's turn.";
            else if (pTurn == 2)
                this.turnLabel.Text = "Player 2's turn.";

            int[] currentValues = new int[cells.Length];
            for (int i = 0; i < cells.Length; i++){
                currentValues[i] = cells[i];
            }
            currentPrevIndex++;
            previousCells.Add(currentValues);

        }

        private bool checkGameOver()
        {
            int sum = 0;
            if (pTurn == 2)
            {
                for (int i = 1; i <= 6; i++)
                {
                    if (cells[i] != 0)
                        return false;
                }
                //Game is over, so take all the opposite side cells and add to p2 bank.
                for (int i = 8; i <= 13; i++)
                {
                    sum += cells[i];
                    cells[i] = 0;
                }
                cells[0] += sum;
            }
            else
            {
                for (int i = 8; i <= 13; i++)
                {
                    if (cells[i] != 0)
                        return false;
                }
                //Game is over, so take all the opposite side cells and add to p1 bank.
                for (int i = 1; i <= 6; i++)
                {
                    sum += cells[i];
                    cells[i] = 0;
                }
                cells[7] += sum;
            }

            if (cells[7] > cells[0])
                this.turnLabel.Text = "Game Over! Player 1 won.";
            else if (cells[0] > cells[7])
                this.turnLabel.Text = "Game Over! Player 2 won.";
            else
                this.turnLabel.Text = "Game Over! Tie!";

            gameOver = true;
            pTurn = 0;
            return true;
        }

        public void undo()
        {
            if (currentPrevIndex > 0)
            {
                cells = previousCells.ElementAt(currentPrevIndex);
                previousCells.RemoveAt(currentPrevIndex);
                currentPrevIndex--;
            }
        }
   

        private void onClick(int cellNum)
        {
            //Player 1's turn
            if (!gameOver && pTurn == 1 && cellNum >= 1 && cellNum <= 6) {

                amt = cells[cellNum]; //Gets total amount of pebbles in cell.
                cells[cellNum] = 0; //Clears the current cell.
                currCell = cellNum + 1;

                pTurn = 2; //Sets to other players turn.
                for (int pebbles = amt; pebbles > 0; pebbles--)
                {
                    switch (currCell)
                    {
                        case 7: //In Player 1's store.
                            if (pebbles == 1)
                                pTurn = 1; //Get another turn if last pebble in your store.
                            break;
                        case 0: //In Player 2's store, skip it.
                            currCell = 1;
                            break;
                        case 14: //Should wrap back to 0.
                            currCell = 0;
                            break;
                    }
                    //If last pebble in empty slot on your side.
                    if (pebbles == 1 && cells[currCell] == 0 && currCell >= 1 && currCell <= 6 )
                    {
                        cells[7]++; //Move the pebble to the store.
                        cells[7] += cells[-currCell+14]; //Steal opposite side pebbles.
                        cells[-currCell + 14] = 0; //Clear opposite side pebbles.
                    }
                    else
                    {
                        cells[currCell]++; //Add one pebble to the current cell.
                        currCell++; //Move on to the next cell.
                    }
                }
            //Player 2's turn
            }else if (gameOver == false && pTurn == 2 && cellNum >= 8) {

                amt = cells[cellNum]; //Gets total amount of pebbles in cell.
                cells[cellNum] = 0; //Clears the current cell.
                currCell = cellNum + 1;

                pTurn = 1; //Sets next turn to player 1. Can be affected later in code.
                for (int pebbles = amt; pebbles > 0; pebbles--)
                {
                    switch (currCell)
                    {
                        case 7: //In player 1's store. Skip to next cell.
                            currCell = 8;
                            break;
                        case 14: //The cell should be 0(Player 2's store), not 14. It wraps around.
                            currCell = 0;
                            if (pebbles == 1)
                            {
                                pTurn = 2; //Get another turn if last pebble
                            }
                            break;
                    }

                    if (pebbles == 1 && cells[currCell] == 0 && currCell >= 8 && currCell <= 13)
                    {
                        cells[0]++; //Move the pebble to the store.
                        cells[0] += cells[-currCell + 14]; //Steal opposite side pebbles.
                        cells[-currCell + 14] = 0; //Clear opposite side pebbles.
                    }
                    else
                    {
                        cells[currCell]++; //Add one pebble to the current cell.
                        currCell++; //Move on to the next cell.
                    }
                }
            }
            checkGameOver(); 
            updateButtons();
       
        }


        private void S1_Click(object sender, EventArgs e)
        {
            onClick(0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            createList();

            int[] currentValues = new int[cells.Length];
            for (int i = 0; i < cells.Length; i++)
            {
                currentValues[i] = cells[i];
            }
            previousCells.Add(currentValues);
        }

        private void A5_Click(object sender, EventArgs e)
        {
            onClick(5);
        }

        private void A4_Click(object sender, EventArgs e)
        {
            onClick(4);
        }

        private void A3_Click(object sender, EventArgs e)
        {
            onClick(3);
        }

        private void A2_Click(object sender, EventArgs e)
        {
            onClick(2);
        }

        private void A1_Click(object sender, EventArgs e)
        {
            onClick(1);
        }

        private void A6_Click(object sender, EventArgs e)
        {
            onClick(6);
        }

        private void S2_Click(object sender, EventArgs e)
        {
            onClick(7);
        }

        private void B1_Click(object sender, EventArgs e)
        {
            onClick(8);
        }

        private void B2_Click(object sender, EventArgs e)
        {
            onClick(9);
        }

        private void B3_Click(object sender, EventArgs e)
        {
            onClick(10);
        }

        private void B4_Click(object sender, EventArgs e)
        {
            onClick(11);
        }

        private void B5_Click(object sender, EventArgs e)
        {
            onClick(12);
        }

        private void B6_Click(object sender, EventArgs e)
        {
            onClick(13);
        }

        public static bool isShown = false;
     

        private void button2_Click(object sender, EventArgs e)
        {
            if (!isShown)
            {
                infoForm form = new infoForm();
                form.Show();

                isShown = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            newGame();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            undo();
        }

    }
}
