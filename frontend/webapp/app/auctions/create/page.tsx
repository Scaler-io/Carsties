import Heading from "@/app/components/heading/heading";
import React from "react";
import AuctionForm from "../auction-form";

const CreateAuctions = () => {
  return (
    <div className="mx-auto max-w-[75%] shadow-lg p-10 bg-white rounded-lg">
      <Heading
        title="Sell you car!"
        subtitle="Please enter the details of your car"
      />
      <AuctionForm />
    </div>
  );
};

export default CreateAuctions;
