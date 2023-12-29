import React from "react";
import AuctionCard from "./auction-card";

const getData = async () => {
  const res = await fetch("http://localhost:4003/search?pageSize=10");

  if (!res.ok) throw new Error("Failed to fetch data");

  return res.json();
};

const Listing = async () => {
  const data = await getData();

  return (
    <div className="grid grid-cols-4 gap-6">
      {data &&
        data.data.map((auction: any) => (
          <AuctionCard key={auction.id} auction={auction} />
        ))}
    </div>
  );
};

export default Listing;
