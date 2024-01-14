import Heading from "@/app/components/heading/heading";
import CountdownTimer from "@/app/components/timer/countdown-timer";
import { getAuctionDetailedView } from "@/app/services/auction.service";
import React from "react";
import CarImage from "../../car-image";
import DetailsSpec from "./details-spec";
import { getCurrentUser } from "@/app/services/auth.service";
import { Button } from "flowbite-react";
import Link from "next/link";

const AuctionDetails = async ({ params }: { params: { id: string } }) => {
  const data = await getAuctionDetailedView(params.id);
  const user = await getCurrentUser();

  return (
    <div>
      <div className="flex justify-between">
        <div className="flex items-center gap-3">
          <Heading title={`${data.item.make} ${data.item.model}`}></Heading>
          {user?.username === data.seller && (
            <Button outline color="info">
              <Link href={`/auctions/update/${data.id}`}>Update Auction</Link>
            </Button>
          )}
        </div>
        <div className="flex gap-3">
          <h3 className="text-xl font-semibold">Time remaining:</h3>
          <CountdownTimer auctionEnd={data.auctionEnd} />
        </div>
      </div>

      <div className="grid grid-cols-2 gap-6 mt-3">
        <div className="w-full bg-gray-200 aspect-h-10 aspect-w-16 wounded-lg overflow-hidden">
          <CarImage imageUrl={data.item.imageUrl} />
        </div>
        <div className="border-2 rounded-lg p-2 bg-gray-100">
          <Heading title="Bids" />
        </div>
      </div>

      <div className="mt-3 grid grid-cols-1 rounded-lg">
        <DetailsSpec auction={data} />
      </div>
    </div>
  );
};

export default AuctionDetails;
