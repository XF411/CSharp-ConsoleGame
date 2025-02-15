using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Learn
{

    public enum MoveDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    public enum SnakeState
    {
        Alive,
        Dead
    }


    public class Snake
    {
        private int birthLength = 5;
        private MoveDirection defaultMoveDirection = MoveDirection.Right;
        private int birthPosX;
        private int birthPosY;
        bool firstDraw;

        /// <summary>
        /// Last body tile need to clean
        /// </summary>
        private int needCleanPosX;
        private int needCleanPosY;

        public SnakeState CurState { get; set; }

        public MoveDirection CurDirection { get; set; }

        public int CurLength { get; set; }

        public List<SnakeBody> Bodies { get; set; }

        public Snake(int x,int y) 
        {
            birthPosX = x;
            birthPosY = y;
            Bodies = new List<SnakeBody>();
            
            for (int i = 0; i < birthLength; i++)
            {
                SnakeBody body = new SnakeBody();
                if (i == 0)
                {
                    body.Type = BodyType.Head;
                }
                else if (i == birthLength - 1)
                {
                    body.Type = BodyType.Tail;
                }
                else
                {
                    body.Type = BodyType.Body;
                }

                switch (defaultMoveDirection)
                {
                    case MoveDirection.Up:
                        body.UpdatePosition(birthPosX, birthPosY - i);
                        break;
                    case MoveDirection.Down:
                        body.UpdatePosition(birthPosX, birthPosY + i);
                        break;
                    case MoveDirection.Left:
                        body.UpdatePosition(birthPosX + i , birthPosY);
                        break;
                    case MoveDirection.Right:
                        body.UpdatePosition(birthPosX - i, birthPosY);
                        break;
                }
                Bodies.Add(body);
            }
            CurLength = Bodies.Count;
            CurState = SnakeState.Alive;
            CurDirection = MoveDirection.Right;
            needCleanPosX = -1;
            needCleanPosY = -1;
            firstDraw = true;
        }

        public void Grow() 
        {

        }

        public void Turn(MoveDirection newDir) 
        {
            if (    newDir == CurDirection
                || (CurDirection == MoveDirection.Up && newDir == MoveDirection.Down)
                || (CurDirection == MoveDirection.Down && newDir == MoveDirection.Up)
                || (CurDirection == MoveDirection.Left && newDir == MoveDirection.Right)
                || (CurDirection == MoveDirection.Right && newDir == MoveDirection.Left)
                ) 
            {
                return;
            }
            CurDirection = newDir;
        }

        public void Move() 
        { 
            switch (CurDirection)
            {
                case MoveDirection.Up:
                    Bodies[0].UpdatePosition(Bodies[0].CurPosX, Bodies[0].CurPosY - 1);
                    break;
                case MoveDirection.Down:
                    Bodies[0].UpdatePosition(Bodies[0].CurPosX, Bodies[0].CurPosY + 1);
                    break;
                case MoveDirection.Left:
                    Bodies[0].UpdatePosition(Bodies[0].CurPosX - 1, Bodies[0].CurPosY);
                    break;
                case MoveDirection.Right:
                    Bodies[0].UpdatePosition(Bodies[0].CurPosX + 1, Bodies[0].CurPosY);
                    break;
            }
            for (int i = 1; i < Bodies.Count; i++)
            {
                Bodies[i].UpdatePosition(Bodies[i - 1].LastPosX, Bodies[i - 1].LastPosY);
            }
            if (!firstDraw)
            {
                needCleanPosX = Bodies[Bodies.Count - 1].LastPosX;
                needCleanPosY = Bodies[Bodies.Count - 1].LastPosY;
            }

        }


        public void Draw()
        {
            if (needCleanPosX != -1 && needCleanPosY != -1)
            {
                ConsoleControler.DrawString(needCleanPosX, needCleanPosY, " ", ConsoleColor.Black);
            }
            foreach (var body in Bodies)
            {
                body.Draw();
            }
            firstDraw = false;
        }

    }
}
