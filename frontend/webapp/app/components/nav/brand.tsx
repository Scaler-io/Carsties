"use client";

import React from "react";
import { useParamsStore } from "@/hooks/useParamsStore";
import { usePathname } from "next/navigation";
import { useRouter } from "next/navigation";
import { AiOutlineCar } from "react-icons/ai";

const Brand = () => {
  const router = useRouter();
  const pathName = usePathname();
  const resetParams = useParamsStore((state) => state.reset);

  const doRestore = () => {
    if (pathName !== "/") router.push("/");
    resetParams();
  };

  return (
    <div
      onClick={doRestore}
      className="cursor-pointer max-w-[25%] flex items-center gap-x-2 text-3xl font-semibold text-accentBlue flex-wrap">
      <AiOutlineCar size={34} />
      <div>Carsties Auction</div>
      <div className="w-full text-xs ml-11 tracking-widest">
        Powered by ACKO Private Limited
      </div>
    </div>
  );
};

export default Brand;
