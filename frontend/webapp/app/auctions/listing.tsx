import React from "react";
import AuctionCard from "./auction-card";
import { getAuctions } from "../services/auction.service";

const Listing = async () => {
  const data = await getAuctions();
  return (
    <div className="grid grid-cols-4 gap-6">
      {data &&
        data.data.map((auction) => (
          <AuctionCard key={auction.id} auction={auction} />
        ))}
    </div>
  );
};

export default Listing;
