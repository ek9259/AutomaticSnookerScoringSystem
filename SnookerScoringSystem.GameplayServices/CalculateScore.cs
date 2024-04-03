using SnookerScoringSystem.GameplayServices.Interfaces;
using SnookerScoringSystem.Domain;
using SnookerScoringSystem.UseCases.Interfaces;
using SnookerScoringSystem.UseCases.PluginInterfaces;

using Player = SnookerScoringSystem.Domain.Player;
using Microsoft.Maui.Controls;
using System.Runtime.CompilerServices;

namespace SnookerScoringSystem.GameplayServices
{
    // All the code in this file is included in all platforms.

    public partial class CalculateScore : ICalculateScore
    {
            private enum BallColour
            {
                Redball = 0,
                Yellowball = 1,
                Greenball = 2,
                Brownball = 3,
                Whiteball = 4,
                Blueball = 5,
                Pinkball = 6,
                Blackball = 7,
                Nullball = 8
            }

            private enum Fouling
            {
                CueballPocketed,
                FoulStroke
            }

        //Global Variable Initialization
            public List<int> PlayersScores = [ 0, 0 ];
            int[] BallScores = { 1, 2, 3, 4, 4, 5, 6, 7 };
            private bool[] IsBallPocketed = { false, false, false, false, false, false, false, false };
            private int CurrentRedBallAmount = 15;
            private int PreviousRedBallAmount = 15;
            private int PlayerTurn = 0;
            private bool Pocketed = false;
            private bool FoulingTurn = false;
            private bool MustHitRed = true;
            private bool PlayerTurnStarted = false;
            private bool ResetingScore = false;
            private Dictionary<DetectedBall, BallLocation> LastLocationInfo = new();

            public void ResetScore()
            {
                PlayersScores = [0, 0];
                for (int i = 0; i < 8; i++)
                {
                    IsBallPocketed[i] = false;
                } 
                CurrentRedBallAmount = 15;
                PreviousRedBallAmount = 15;
                PlayerTurn = 0;
                Pocketed = false;
                FoulingTurn = false;
                MustHitRed = true;
                PlayerTurnStarted = false;
                ResetingScore = true;
                LastLocationInfo.Clear();
            }

            public async Task <List<int>> CalculateScoreAsync(List<DetectedBall> detectedBalls)
            {
            if (!ResetingScore)
            {
                if (LastLocationInfo.Count != 0)
                {
                    //Player Turn Logic
                    //If any ball moved, then the player turn is started.
                    if (IsBallMoved(detectedBalls, (int)BallColour.Whiteball) && !PlayerTurnStarted)
                    {
                        PlayerTurnStarted = true;
                        FoulingTurn = false;
                    }
                    //If all balls are stopped, the player continue the player turn if the player sucecssfully Pocketed a ball.
                    if (!IsAnyBallMoved(detectedBalls, (int)BallColour.Nullball) && !IsAnyRedBallMoved(detectedBalls) && PlayerTurnStarted)
                    {
                        PlayerTurnStarted = false;
                        /*If no ball are Pocketed, the player turn will switch to next player.
                         * Player 1 = 0, Player 2 = 1
                         */
                        if (!Pocketed)
                        {
                            if (!FoulingTurn)
                            {
                                switch (PlayerTurn)
                                {
                                    case 0:
                                        PlayerTurn = 1;
                                        break;
                                    case 1:
                                        PlayerTurn = 0;
                                        break;
                                }
                            }

                            if (!MustHitRed)
                                MustHitRed = true;
                        }
                        Pocketed = false;
                    }
                    if (AnyBallPockectedEvent(detectedBalls))
                    {
                        Pocketed = true;
                    }
                }
                //Clear all the saved location information from the previous frame and then start saving new location information of the current frame.
                LastLocationInfo.Clear();
                foreach (var Obj in detectedBalls)
                {
                    LastLocationInfo.Add(Obj, new BallLocation(Obj.X, Obj.Y));
                }
                return PlayersScores;
            }
            ResetingScore = false;
            return PlayersScores;
            }

            //Threshold for the ball location differences.
            private bool notEQ(double tempdouble1, double tempdouble2)
            {
                var tempdouble = tempdouble1 - tempdouble2;
                return tempdouble < -5d || tempdouble > 5d;
            }
            private bool AnyBallPockectedEvent(List<DetectedBall> detectedBalls)
            {
                //Specific Case for red ball
                int BallPockected = 0;
                bool RedPocketed = false;
                bool ColoredBallPocketed = false;
                int TotalScores = 0;

                //All ball information in previous frame
                Dictionary<int, int> count_bef = new();
                for (int i = (int)BallColour.Redball; i <= (int)BallColour.Blackball; i++)
                {
                    count_bef.Add(i, 0);
                }
                foreach (var Obj in LastLocationInfo)
                {
                    if (count_bef.ContainsKey(Obj.Key.ClassId))
                    {
                        count_bef[Obj.Key.ClassId]++;
                    }
                }

                //All ball information in current frame
                Dictionary<int, int> count_cur = new();
                for (int i = (int)BallColour.Redball; i <= (int)BallColour.Blackball; i++)
                {
                    count_cur.Add(i, 0);
                }
                foreach (var Obj in detectedBalls)
                {
                    if (count_cur.ContainsKey(Obj.ClassId))
                    {
                        count_cur[Obj.ClassId]++;
                    }
                }

                for (int i = (int)BallColour.Redball; i <= (int)BallColour.Blackball; i++)
                {
                    // Ball Return to table
                    if (count_cur[i] > count_bef[i])
                    {
                        if (i != (int)BallColour.Redball && IsBallPocketed[i] && count_cur[i] > 0)
                        {
                            IsBallPocketed[i] = false;
                        }
                    }
                    // skip whiteball and calculate the ball amount from previous frame and current frame, if more ball in previous frame than current, then a ball is missing (Pocketed)
                    if (count_cur[i] < count_bef[i])
                    {
                        //Redball Pocketed
                        if (i == (int)BallColour.Redball && PreviousRedBallAmount > count_cur[i])
                        {
                            RedPocketed = true;
                            TotalScores += BallScores[i] * (count_bef[i] - count_cur[i]);
                            if (MustHitRed)
                            {
                                BallPockected += count_bef[i] - count_cur[i];
                                PreviousRedBallAmount--;
                                if (BallScores[i] * (count_bef[i] - count_cur[i]) > 0)
                                {
                                    PlayersScores[PlayerTurn] += BallScores[i] * (count_bef[i] - count_cur[i]);
                                }
                            }
                        }

                        // Other colour ball Pocketed

                        if (!IsBallPocketed[i] && i != (int)BallColour.Redball && i != (int)BallColour.Whiteball && (count_cur[i] < 2 && count_bef[i] < 2))
                        {
                            ColoredBallPocketed = true;
                            TotalScores += TotalScores += BallScores[i] * (count_bef[i] - count_cur[i]);
                            IsBallPocketed[i] = true;
                            if (!MustHitRed)
                            {
                                BallPockected += count_bef[i] - count_cur[i];
                                if (BallScores[i] * (count_bef[i] - count_cur[i]) > 0)
                                {
                                    PlayersScores[PlayerTurn] += BallScores[i] * (count_bef[i] - count_cur[i]);
                                }
                            }
                        }
                        // Fouling if a cueball is Pocketed (Cueball Pocketed)
                        if (!IsBallPocketed[i] && i == (int)BallColour.Whiteball && (count_cur[i] < 2 && count_bef[i] < 2))
                        {
                            CheckFouling(Fouling.CueballPocketed, 4);
                            IsBallPocketed[i] = true;
                        }

                        //Fouling (Foul Stroke)

                        if ((ColoredBallPocketed && MustHitRed) || (RedPocketed && !MustHitRed))
                        {
                            CheckFouling(Fouling.FoulStroke, TotalScores);
                        }

                        if (RedPocketed && MustHitRed)
                        {
                            MustHitRed = false;
                        }
                        else if (ColoredBallPocketed && !MustHitRed)
                        {
                            MustHitRed = true;
                        }
                    }
                }

                return BallPockected > 0;
            }

            //Detect Specific Ball Movement by comparing the XY saved in previous frame with current frame XY using dictionary with primary key as ball class and secondary key as location struct
            private bool IsBallMoved(List<DetectedBall> detectedBalls, int ClassID)
            {
                foreach (var Obj in detectedBalls)
                {
                    if (Obj.ClassId == ClassID)
                    {
                        foreach (var SavedObjects in LastLocationInfo)
                        {
                            if (SavedObjects.Key.ClassId == ClassID)
                                if (
                                    Obj != null && (
                                    notEQ(Obj.X, SavedObjects.Key.X) ||
                                    notEQ(Obj.Y, SavedObjects.Key.Y)
                                ))
                                {
                                    return true;
                                }
                        }
                    }
                }
                return false;
            }
            //Detect any by comparing the XY saved in previous frame with current frame XY using dictionary with primary key as ball class and secondary key as location struct
            private bool IsAnyBallMoved(List<DetectedBall> detectedBalls, int WhichBalltoIgnore)
            {
                foreach (var Obj in detectedBalls)
                {
                    if (Obj.ClassId > (int)BallColour.Redball && Obj.ClassId <= (int)BallColour.Blackball)
                    {
                        foreach (var SavedObjects in LastLocationInfo)
                        {
                            if (SavedObjects.Key.ClassId > (int)BallColour.Redball && SavedObjects.Key.ClassId <= (int)BallColour.Blackball && SavedObjects.Key.ClassId != WhichBalltoIgnore)
                                if (
                                    Obj != null && Obj.ClassId == SavedObjects.Key.ClassId && (
                                    notEQ(Obj.X, SavedObjects.Key.X) ||
                                    notEQ(Obj.Y, SavedObjects.Key.Y)
                                ))
                                {
                                    return true;
                                }
                        }
                    }
                }
                return false;
            }

            //Detect Redball Movement by comparing the XY saved in previous frame with current frame XY using dictionary with primary key as ball class and secondary key as location struct
            private bool IsAnyRedBallMoved(List<DetectedBall> detectedBalls)
            {
                foreach (var Obj in detectedBalls)
                {
                    if (Obj.ClassId == (int)BallColour.Redball)
                    {
                        bool Moved = true;
                        foreach (var SavedObjects in LastLocationInfo)
                        {
                            if (Obj != null && SavedObjects.Key.ClassId == (int)BallColour.Redball && (
                                !notEQ(Obj.X, SavedObjects.Value.X) &&
                                !notEQ(Obj.Y, SavedObjects.Value.Y)
                            ))
                            {
                                Moved = false;
                            }
                        }
                        if (Moved)
                            return true;
                    }
                }
                return false;
            }
            //Fouling
                private void CheckFouling(Fouling FoulingType, int Score)
                {
                    FoulingTurn = true;
                    switch (FoulingType)
                    {
                        /*Fouling 1: FoulStroke
                         * When a player Pocketed the ball in wrong order.
                         */
                        case Fouling.FoulStroke:
                            switch (PlayerTurn)
                            {
                                case 0:
                                    PlayerTurn = 1;
                                    break;
                                case 1:
                                    PlayerTurn = 0;
                                    break;
                            }
                            PlayersScores[PlayerTurn] += Score;
                            break;
                        /*Fouling 2: Cueball Pocketed
                         * When a player Pocketed the cueball (White ball).
                         */
                        case Fouling.CueballPocketed:
                            switch (PlayerTurn)
                            {
                                case 0:
                                    PlayerTurn = 1;
                                    break;
                                case 1:
                                    PlayerTurn = 0;
                                    break;
                            }
                            PlayersScores[PlayerTurn] += Score;
                            break;
                    }

                //If the fouling committed increase the opponent's score.
               }
    }
}