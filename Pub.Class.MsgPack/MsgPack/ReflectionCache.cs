﻿//
// Copyright 2011 Kazuki Oikawa
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using System;
using System.Collections.Generic;
using System.Collections;

namespace MsgPack {
    public static class ReflectionCache {
        static Dictionary<Type, ReflectionCacheEntry> _cache;

        static ReflectionCache() {
            _cache = new Dictionary<Type, ReflectionCacheEntry>();
        }

        public static ReflectionCacheEntry Lookup(Type type) {
            ReflectionCacheEntry entry;
            lock (((ICollection)_cache).SyncRoot) {
                if (_cache.TryGetValue(type, out entry))
                    return entry;
            }

            entry = new ReflectionCacheEntry(type);
            lock (((ICollection)_cache).SyncRoot) {
                _cache[type] = entry;
            }
            return entry;
        }

        public static void RemoveCache(Type type) {
            lock (((ICollection)_cache).SyncRoot) {
                _cache.Remove(type);
            }
        }

        public static void Clear() {
            lock (((ICollection)_cache).SyncRoot) {
                _cache.Clear();
            }
        }
    }
}
