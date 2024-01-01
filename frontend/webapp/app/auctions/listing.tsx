"use client";
import React, { useEffect, useState } from "react";
import AuctionCard from "./auction-card";
import AppPagination from "../components/pagination/app-pagination";
import { getAuctions } from "../services/auction.service";
import { Auction } from "../models/auction";
import Filter from "./filter";

const Listing = () => {
  const [auctions, setAuctions] = useState<Auction[]>([]);
  const [pageCount, setPageCount] = useState(0);
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize, setPageSize] = useState(4);

  useEffect(() => {
    getAuctions(pageNumber, pageSize).then((data) => {
      setAuctions(data.data);
      setPageCount(data.pageCount);
    });
  }, [pageNumber, pageSize]);

  if (auctions.length === 0) return <h3>Loading...</h3>;

  return (
    <>
      <Filter pageSize={pageSize} setPageSize={setPageSize} />
      <div className="grid grid-cols-4 gap-6">
        {auctions.map((auction) => (
          <AuctionCard key={auction.id} auction={auction} />
        ))}
      </div>
      <div className="flex justify-center mt-4">
        <AppPagination
          pageChanged={setPageNumber}
          currentPage={pageNumber}
          pageCount={pageCount}
        />
      </div>
    </>
  );
};

export default Listing;
