﻿Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel
Imports Connector = Microsoft.VisualBasic.MachineLearning.NeuralNetwork.Synapse

Namespace NeuralNetwork.StoreProcedure

    ''' <summary>
    ''' 对于大型的神经网络而言，反复的构建XML数据模型将会额外的消耗掉大量的时间，导致训练的时间过长
    ''' 在这里通过这个持久性的快照对象来减少这种反复创建XML数据快照的问题
    ''' </summary>
    Public Class Snapshot

        ReadOnly snapshot As NeuralNetwork
        ReadOnly source As Network

        ''' <summary>
        ''' 节点的数量较少
        ''' </summary>
        ReadOnly neuronLinks As Map(Of Neuron, NeuronNode)()
        ''' <summary>
        ''' 因为链接的数量非常多，可能会超过了一个数组的元素数量上限，
        ''' 所以在这里使用多个分组来避免这个问题
        ''' </summary>
        ReadOnly synapseLinks As Map(Of Connector, Synapse)()()

        Sub New(model As Network)
            source = model
            snapshot = CreateSnapshot.TakeSnapshot(model, 0)
        End Sub

        Private Shared Function createNeuronUpdateMaps(source As Network, snapshot As NeuralNetwork) As Map(Of Neuron, NeuronNode)()

        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="error">
        ''' The calculation errors of current snapshot.
        ''' </param>
        ''' <returns></returns>
        Public Function UpdateSnapshot([error] As Double) As Snapshot
            Dim toNode As NeuronNode
            Dim fromNode As Neuron

            snapshot.errors = [error]
            snapshot.learnRate = source.LearnRate
            snapshot.momentum = source.Momentum

            ' update node and links in current neuron network
            For Each node In neuronLinks
                toNode = node.Maps
                fromNode = node.Key

                toNode.bias = fromNode.Bias
                toNode.delta = fromNode.BiasDelta
                toNode.gradient = fromNode.Gradient
                toNode.id = fromNode.Guid
            Next

            Dim toLink As Synapse
            Dim fromLink As Connector

            For Each layer In synapseLinks
                For Each connection In layer
                    toLink = connection.Maps
                    fromLink = connection.Key

                    toLink.delta = fromLink.WeightDelta
                    toLink.w = fromLink.Weight
                Next
            Next

            Return Me
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function WriteIntegralXML(path As String) As Boolean
            Return snapshot.GetXml.SaveTo(path, throwEx:=False)
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function WriteScatteredParts(directory As String) As Boolean
            Return snapshot.ScatteredStore(store:=directory)
        End Function

        Public Overrides Function ToString() As String
            Return source.ToString
        End Function
    End Class
End Namespace