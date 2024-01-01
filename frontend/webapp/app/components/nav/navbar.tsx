'use client';

import React from "react";
import { AiOutlineCar } from "react-icons/ai";
import SearchBar from "./searchbar";
import { useParamsStore } from "@/hooks/useParamsStore";

type NavbarProps = {
  title?: string;
};

const Navbar = ({ title }: NavbarProps) => {
  const resetParams = useParamsStore((state) => state.reset);

  return (
    <header
      className="
        sticky top-0 z-50 flex justify-evenly bg-white p-5 items-center text-gray-800 shadow-sm
      ">
      <div
        onClick={resetParams}
        className="cursor-pointer max-w-[25%] flex items-center gap-x-2 text-3xl font-semibold text-accentBlue flex-wrap">
        <AiOutlineCar size={34} />
        <div>{title}</div>
        <div className="w-full text-xs ml-11 tracking-widest">
          Powered by ACKO Private Limited
        </div>
      </div>

      <SearchBar />

      <div>Login</div>
    </header>
  );
};

export default Navbar;
