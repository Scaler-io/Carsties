"use client";
import Heading from "@/app/components/heading/heading";
import React from "react";
import AuctionForm from "../auction-form";
import { createAuction } from "@/app/services/auction.service";
import { useRouter } from "next/navigation";
import toast from "react-hot-toast";

const CreateAuctions = () => {
  const router = useRouter();
  
  // handles create auction form actions
  const onSubmit = async (formData) => {
    try {
      const res = await createAuction(formData);
      if (res.error) {
        throw res.error;
      }
      router.push(`/auctions/details/${res.id}`);
    } catch (error) {
      toast.error(error.status + " " + error.message);
    }
  };

  return (
    <div className="mx-auto max-w-[75%] shadow-lg p-10 bg-white rounded-lg">
      <Heading title="Sell you car!" subtitle="Please enter the details of your car"/>
      <AuctionForm onNextClick={onSubmit} />
    </div>
  );
};

export default CreateAuctions;
