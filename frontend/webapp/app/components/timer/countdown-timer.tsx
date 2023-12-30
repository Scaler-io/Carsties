"use client";
import React from "react";
import Countdown, { zeroPad } from "react-countdown";

interface RendererProp{
  days: number;
  hours: number;
  minutes: number;
  seconds: number;
  completed: boolean;
};

interface AuctionProp{
  auctionEnd: string;
};

const renderer = ({
  days,
  hours,
  minutes,
  seconds,
  completed,
}: RendererProp) => {
  return (
    <div
      className={`border-2 py-1 px-2 text-white border-white rounded-lg flex justify-center text-sm transition-all delay-75
      ${
        completed
          ? "bg-lightRed"
          : days === 0 && hours < 10
          ? "bg-amber-600"
          : "bg-accentBlue"
      }`}>
      {completed ? (
        <span>Auction finished</span>
      ) : (
        <span suppressHydrationWarning={true}>
          {zeroPad(days)}:{zeroPad(hours)}:{minutes}:{seconds} Left
        </span>
      )}
    </div>
  );
};

const CountdownTimer = ({ auctionEnd }: AuctionProp) => {
  return (
    <div>
      <Countdown date={auctionEnd} renderer={renderer} />
    </div>
  );
};

export default CountdownTimer;
