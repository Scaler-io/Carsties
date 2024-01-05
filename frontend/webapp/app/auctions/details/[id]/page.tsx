import React from "react";

const AuctionDetails = ({ params }: { params: { id: string } }) => {
  return <div>Details for {params.id}</div>;
};

export default AuctionDetails;
