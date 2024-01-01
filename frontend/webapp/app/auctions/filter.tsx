import { useParamsStore } from "@/hooks/useParamsStore";
import { Button } from "flowbite-react";
import React from "react";
import {
  filterButtons,
  orderButtons,
  pageSizeOptions,
} from "../models/filter-options";

const Filter = () => {
  const setParams = useParamsStore((state) => state.setParams);
  const pageSize = useParamsStore((state) => state.pageSize);
  const orderBy = useParamsStore((state) => state.orderBy);
  const filterBy = useParamsStore((state) => state.filterBy);

  return (
    <div className="flex justify-between items-center mb-4">
      {/* Filter options */}
      <div>
        <span className="uppercase text-sm text-gray-200 mr-2">Filters</span>
        <Button.Group>
          {filterButtons.map(({ label, icon: Icon, value }) => (
            <Button
              key={value}
              onClick={() => setParams({ filterBy: value })}
              color={`${filterBy === value ? "info" : "gray"}`}
              className="focus:ring-0">
              <Icon className="mr-3 h-4" />
              {label}
            </Button>
          ))}
        </Button.Group>
      </div>

      {/* Sorting options */}
      <div>
        <span className="uppercase text-sm text-gray-200 mr-2">Sort</span>
        <Button.Group>
          {orderButtons.map(({ label, icon: Icon, value }) => (
            <Button
              key={value}
              onClick={() => setParams({ orderBy: value })}
              color={`${orderBy === value ? "info" : "gray"}`}
              className="focus:ring-0">
              <Icon className="mr-3 h-4" />
              {label}
            </Button>
          ))}
        </Button.Group>
      </div>

      {/* page size options */}
      <div>
        <span className="uppercase text-sm text-gray-500 mr-2">Page size</span>
        <Button.Group>
          {pageSizeOptions.map((value, i) => (
            <Button
              key={i}
              onClick={() => {
                setParams({ pageSize: value });
              }}
              color={`${pageSize === value ? "info" : "gray"}`}
              className="focus:ring-0">
              {value}
            </Button>
          ))}
        </Button.Group>
      </div>
    </div>
  );
};

export default Filter;
