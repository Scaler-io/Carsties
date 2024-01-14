"use client";

import React, { useEffect, useState } from "react";
import AuctionCard from "./auction-card";
import AppPagination from "../components/pagination/app-pagination";
import { getAuctions } from "../services/auction.service";
import { PageResult } from "../models/page-result";
import { useParamsStore } from "../../hooks/useParamsStore";
import { shallow } from "zustand/shallow";
import qs from "query-string";
import EmptyFilter from "../components/empty-filter/empty-filter";
import Loader from "../components/loader/loader";
import PageActions from "./page-actions/page-actions";
import { AuctionSearch } from "../models/auction-serach";

const Listing = () => {
  const [result, setResult] = useState<PageResult<AuctionSearch>>();
  const params = useParamsStore(
    (state) => ({
      pageNumber: state.pageNumber,
      pageSize: state.pageSize,
      searchTerm: state.searchTerm,
      orderBy: state.orderBy,
      filterBy: state.filterBy,
      seller: state.seller,
      winner: state.winner,
    }),
    shallow
  );
  const setParams = useParamsStore((state) => state.setParams);
  const url = qs.stringifyUrl({ url: "", query: params });

  const setPageNumber = (pageNumber: number) => {
    setParams({ pageNumber });
  };

  useEffect(() => {
    getAuctions(url).then((result) => {
      setResult(result);
    });
  }, [url]);

  if (!result) return <Loader />;

  return (
    <>
      <PageActions />

      {result.total === 0 ? (
        <EmptyFilter showReset />
      ) : (
        <>
          <div className="grid grid-cols-4 gap-6 mt-24">
            {result.data.map((auction) => (
              <AuctionCard key={auction.id} auction={auction} />
            ))}
          </div>
          <div className="flex justify-center mt-4">
            <AppPagination
              pageChanged={setPageNumber}
              currentPage={params.pageNumber}
              pageCount={result.pageCount}
            />
          </div>
        </>
      )}
    </>
  );
};

export default Listing;
