﻿using System;
using KelpNet.Common;
using KelpNet.Common.Functions.Container;
using KelpNet.Common.Tools;
using KelpNet.Functions.Activations;
using KelpNet.Functions.Connections;
using KelpNet.Loss;
using KelpNet.Optimizers;

namespace KelpNetTester.Tests
{
    //Learning of XOR by MLP 【Regression version】 ※ The precision is bad and it is not possible to obtain the desired result unless it is executed several times
    class Test2
    {
        public static void Run()
        {
            //Number of exercises
            const int learningCount = 10000;

            Real[][] trainData =
            {
                new Real[] { 0, 0 },
                new Real[] { 1, 0 },
                new Real[] { 0, 1 },
                new Real[] { 1, 1 }
            };

            Real[][] trainLabel =
            {
                new Real[] { 0 },
                new Real[] { 1 },
                new Real[] { 1 },
                new Real[] { 0 }
            };

            FunctionStack nn = new FunctionStack(
                new Linear(2, 2, name: "l1 Linear"),
                new ReLU(name: "l1 ReLU"),
                new Linear(2, 1, name: "l2 Linear")
            );

            //nn.SetOptimizer(new Adam());
            nn.SetOptimizer(new AdaGrad());


            Console.WriteLine("Training...");
            for (int i = 0; i < learningCount; i++)
            {
                //use MeanSquaredError for loss function
                Trainer.Train(nn, trainData[0], trainLabel[0], new MeanSquaredError(), false);
                Trainer.Train(nn, trainData[1], trainLabel[1], new MeanSquaredError(), false);
                Trainer.Train(nn, trainData[2], trainLabel[2], new MeanSquaredError(), false);
                Trainer.Train(nn, trainData[3], trainLabel[3], new MeanSquaredError(), false);

                //If you do not update every time after training, you can update it as a mini batch
                nn.Update();
            }

            Console.WriteLine("Test Start...");
            foreach (Real[] val in trainData)
            {
                NdArray result = nn.Predict(val)[0];
                Console.WriteLine(val[0] + " xor " + val[1] + " = " + (result.Data[0] > 0.5 ? 1 : 0) + " " + result);
            }
        }
    }
}
